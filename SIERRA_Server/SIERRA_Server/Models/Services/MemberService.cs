using Humanizer;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop.Infrastructure;
using NuGet.Common;
using NuGet.DependencyResolver;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Repository.DPRepository;
using SIERRA_Server.Models.Repository.EFRepository;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace SIERRA_Server.Models.Services
{
	public class MemberService
	{
		private readonly MemberDPRepository _repo;
		private readonly HashUtility _hashUtility;
		private readonly EmailHelper _emailHelper;
		private readonly IConfiguration _config;
		public MemberService(MemberDPRepository repo, HashUtility hashUtility, EmailHelper emailHelper)
		{
			_repo = repo;
			_hashUtility = hashUtility;
			_emailHelper = emailHelper;
		}
		public MemberService(MemberDPRepository repo, HashUtility hashUtility, IConfiguration config)
		{
			_repo = repo;
			_hashUtility = hashUtility;
			_config = config;
		}
		public MemberService(MemberDPRepository repo, HashUtility hashUtility)
		{
			_repo = repo;
			_hashUtility = hashUtility;
		}

		public MemberService(MemberDPRepository repo, IConfiguration config)
		{
			_repo = repo;
			_config = config;
		}
		public MemberService(MemberDPRepository repo)
		{
			_repo = repo;
		}

		public Result ValidLogin(LoginDTO dto)
		{
			var memberInDb = _repo.GetMemberByUsername(dto.Username);
			if (memberInDb == null) return Result.Fail("帳密有誤");

			if (memberInDb.IsConfirmed == false) return Result.Fail("會員資格尚未確認，請至信箱點選驗證信");

			if (memberInDb.IsBan == true) return Result.Fail("該帳號已被停權，無法登入");

			var salt = _hashUtility.GetSalt();
			var hashPassword = _hashUtility.ToSHA256(dto.Password, salt);

			return string.Compare(memberInDb.EncryptedPassword, hashPassword) == 0
				? Result.Success()
				: Result.Fail("帳密有誤");
		}

		public Result GoogleRegister(RegisterDTO dto)
		{
			// 判斷username是否重複
			var memberInDb = _repo.GetMemberByUsername(dto.Username);
			if (memberInDb != null) return Result.Fail("使用者名稱重複");

			// 因在GoogleLogin()時就已經檢查過email了，故不再檢查

			// 判斷密碼是否符合:長度為8~16，且必須包含至少1個英文字母和1個數字
			if (!Regex.IsMatch(dto.Password, @"^(?=.*[a-zA-Z])(?=.*[0-9]).{8,16}$")) return Result.Fail("密碼長度為8~16，且須含英文字母和數字");

			// 填入剩餘欄位的值
			var salt = _hashUtility.GetSalt();
			dto.EncryptedPassword = _hashUtility.ToSHA256(dto.Password, salt);

			dto.IsConfirmed = true;
			dto.ConfirmCode = null;

			dto.ImageName = "default.png";

			// 新增會員資料
			_repo.PostMember(dto);

			return Result.Success();
		}
		public string? CreateJwtToken(string username)
		{
			var memberInDb = _repo.GetMemberByUsername(username);
			var memberId = memberInDb.Id;
			var imageName = memberInDb.ImageName;
			// 設定使用者資訊
			var claims = new List<Claim>
			{
				new Claim("username",username),
				new Claim("memberId",memberId.ToString()),
				new Claim("imageName",imageName)
			};

			// 取出appsettings.json中的KEY
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:KEY"]));

			// 設定 JWT 相關資訊
			var jwt = new JwtSecurityToken
			(
				issuer: _config["JWT:Issuer"],
				audience: _config["JWT:Audience"],
				claims: claims,
				//expires: DateTime.Now.AddMinutes(30),
				expires: DateTime.Now.AddDays(30),
				signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
			);

			// 產生JWT Token
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
		public Result Register(RegisterDTO dto)
		{
			// 判斷username是否重複
			var memberInDb = _repo.GetMemberByUsername(dto.Username);
			if (memberInDb != null) return Result.Fail("使用者名稱重複");

			// 判斷email是否重複
			if (_repo.IsEmailExist(dto.Email)) return Result.Fail("電子信箱已註冊");

			// 判斷密碼是否符合:長度為8~16，且必須包含至少1個英文字母和1個數字
			if (!Regex.IsMatch(dto.Password, @"^(?=.*[a-zA-Z])(?=.*[0-9]).{8,16}$")) return Result.Fail("密碼長度為8~16，且須含英文字母和數字");

			// 填入剩餘欄位的值
			var salt = _hashUtility.GetSalt();
			dto.EncryptedPassword = _hashUtility.ToSHA256(dto.Password, salt);

			dto.IsConfirmed = false;

			var confirmCode = Guid.NewGuid().ToString("N");
			dto.ConfirmCode = confirmCode;

			dto.ImageName = "default.png";

			// 新增會員資料
			_repo.PostMember(dto);

			// 寄驗證信
			var memberId = _repo.GetMemberIdByUsername(dto.Username);
			//var confirmLink = _urlHelper.Action("ActiveRegister", "Members", new { IdToString, confirmCode });
			var confirmUrl = $"http://localhost:5501/RegisterActive.html?memberId={memberId}&confirmCode={confirmCode}";
			_emailHelper.SendConfirmRegisterEmail(dto.Email, confirmUrl, dto.Username);

			return Result.Success();

		}
		public Result ActiveRegister(ActiveRegisterDTO dto)
		{
			var memberInDb = _repo.GetMemberById(dto.MemberId);
			if (memberInDb == null || memberInDb.ConfirmCode != dto.ConfirmCode) return Result.Fail("驗證錯誤");

			memberInDb.IsConfirmed = true;
			memberInDb.ConfirmCode = null;

			//_repo.SaveChanges();
			_repo.ActiveRegister(memberInDb);


			return Result.Success();
		}
		public Result ForgotPassword(ForgotPasswordDTO dto)
		{
			// 檢查該帳號和Email是否正確
			var memberInDb = _repo.GetMemberByUsername(dto.Username);
			if (memberInDb == null || string.Compare(dto.Email, memberInDb.Email, StringComparison.CurrentCultureIgnoreCase) != 0) return Result.Fail("帳號或Email錯誤");

			// 已啟用的帳號才能重設密碼
			if (memberInDb.IsConfirmed == false) return Result.Fail("尚未啟用本帳號，請先完成才能重設密碼");

			// 填入 confirmCode，更新資料
			var confirmCode = Guid.NewGuid().ToString("N");
			memberInDb.ConfirmCode = confirmCode;

			//_repo.SaveChanges();
			_repo.ForgotPassword(memberInDb);

			// 寄驗證信
			var memberId = _repo.GetMemberIdByUsername(dto.Username);
			var confirmUrl = $"http://localhost:5501/ResetPassword.html?memberId={memberId}&confirmCode={confirmCode}";
			_emailHelper.SendForgotPasswordEmail(dto.Email, confirmUrl, dto.Username);

			return Result.Success();
		}

		public Result ResetPassword(ResetPasswordDTO dto)
		{
			// 檢查該帳號是否存在
			var memberInDb = _repo.GetMemberById(dto.MemberId);
			if (memberInDb == null || memberInDb.ConfirmCode != dto.ConfirmCode) return Result.Fail("重設失敗，請檢查驗證連結是否有更動");

			// 更新密碼，並將ConfirmCode清空
			var salt = _hashUtility.GetSalt();
			var hashPassword = _hashUtility.ToSHA256(dto.NewPassword, salt);

			memberInDb.EncryptedPassword = hashPassword;
			memberInDb.ConfirmCode = null;

			// _repo.SaveChanges();
			_repo.ResetPassword(memberInDb);

			return Result.Success();

		}

		public Result EditPassword(EditPasswordDTO dto)
		{
			var memberInDb = _repo.GetMemberById(dto.MemberId);
			if (memberInDb == null) return Result.Fail("找不到要修改的會員紀錄");

			var salt = _hashUtility.GetSalt();
			var hashOriginalPassword = _hashUtility.ToSHA256(dto.OriginalPassword, salt);
			if (memberInDb.EncryptedPassword != hashOriginalPassword) return Result.Fail("密碼錯誤");

			// 更新密碼
			memberInDb.EncryptedPassword = _hashUtility.ToSHA256(dto.NewPassword, salt);

			//_repo.SaveChanges();
			_repo.EditPassword(memberInDb);

			return Result.Success();
		}

		public async Task<EditMemberDTO> GetMember(int id)
		{
			var memberInDb = await _repo.GetMemberByIdAsync(id);
			if (memberInDb != null)
			{
				var editMemberDTO = new EditMemberDTO
				{
					Id = memberInDb.Id,
					Username = memberInDb.Username,
					Email = memberInDb.Email,
					Address = memberInDb.Address,
					Phone = memberInDb.Phone,
					Birth = memberInDb.Birth,
					Gender = memberInDb.Gender,
					ImageName = memberInDb.ImageName,
				};
				return editMemberDTO;
			}
			return null;
		}
		public async Task<Result> EditMemberAsync(EditMemberDTO dto)
		{
			var memberInDb = await _repo.GetMemberByIdAsync(dto.Id);

			if (memberInDb == null) return Result.Fail("找不到要修改的會員記錄");

			if (!ValidPhone(dto.Phone))
			{
				return Result.Fail("電話號碼格式錯誤");
			}

			memberInDb.Address = dto.Address;
			memberInDb.Phone = string.IsNullOrEmpty(dto.Phone) ? null : dto.Phone;
			memberInDb.Birth = dto.Birth;
			memberInDb.Gender = dto.Gender;

			//await _repo.SaveChangesAsync();
			await _repo.EditMemberAsync(memberInDb);


			return Result.Success();
		}
		private bool ValidPhone(string? phone)
		{
			return string.IsNullOrEmpty(phone) || Regex.IsMatch(phone, @"^\d{10}$|^$");
		}
		public async Task<Result> EditMemberImageAsync(EditMemberImageDTO dto)
		{
			var memberInDb = await _repo.GetMemberByIdAsync(dto.Id);
			if (memberInDb == null) return Result.Fail("找不到要修改的會員記錄");

			string fileName = SaveUploadedFile(dto.UploadFile);
			if (fileName == string.Empty) return Result.Fail("請選擇正確的檔案類型");

			memberInDb.ImageName = fileName;

			// await _repo.SaveChangesAsync();
			await _repo.EditMemberImageAsync(memberInDb);

			return Result.Success();
		}
		private string SaveUploadedFile(IFormFile image)
		{
			// precondition
			if (image == null || image.Length == 0) return string.Empty;
			// 取得上傳檔案的副檔名,".jpg"而不是"jpg"
			string ext = Path.GetExtension(image.FileName);
			// 如果副檔名不在允許的範圍裡,表示上傳不合理的檔案類型,就不處理,傳回string.Empty
			string[] allowedExts = new string[] { ".jpg", ".jpeg", ".png", ".tif" };
			if (allowedExts.Contains(ext.ToLower()) == false) return string.Empty;

			// 生成亂數檔名
			string newFileName = Guid.NewGuid().ToString("N") + ext;
			var directory = Directory.GetCurrentDirectory();
			string filePath = Path.Combine(directory, "Uploads", newFileName);

			//將上傳的檔案存放到指定位置
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				image.CopyTo(stream);
			}

			//傳回存放的檔名
			return newFileName;
		}

		public async Task<string> GetMemberImageName(int id)
		{
			var memberInDb = await _repo.GetMemberByIdAsync(id);
			if (memberInDb == null) return null;
			return memberInDb.ImageName;
		}




	}
}

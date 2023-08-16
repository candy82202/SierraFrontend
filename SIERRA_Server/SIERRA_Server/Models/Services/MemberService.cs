using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop.Infrastructure;
using NuGet.Common;
using NuGet.DependencyResolver;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Repository.EFRepository;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Transactions;

namespace SIERRA_Server.Models.Services
{
	public class MemberService
	{
		private readonly MemberEFRepository _repo;
		private readonly HashUtility _hashUtility;
		private readonly EmailHelper _emailHelper;
		private readonly IConfiguration _config;
		public MemberService(MemberEFRepository repo, HashUtility hashUtility, EmailHelper emailHelper)
		{
			_repo = repo;
			_hashUtility = hashUtility;
			_emailHelper = emailHelper;
		}
		public MemberService(MemberEFRepository repo, HashUtility hashUtility, IConfiguration config)
		{
			_repo = repo;
			_hashUtility = hashUtility;
			_config = config;
		}
		public MemberService(MemberEFRepository repo, HashUtility hashUtility)
		{
			_repo = repo;
			_hashUtility = hashUtility;
		}

		public MemberService(MemberEFRepository repo, IConfiguration config)
		{
			_repo = repo;
			_config = config;
		}
		public MemberService(MemberEFRepository repo)
		{
			_repo = repo;
		}

		public Result ValidLogin(LoginDTO dto)
		{
			var member = _repo.GetMemberByUsername(dto.Username);
			if (member == null) return Result.Fail("帳密有誤");

			if (member.IsConfirmed.HasValue == false || member.IsConfirmed.Value == false) return Result.Fail("會員資格尚未確認");

			var salt = _hashUtility.GetSalt();
			var hashPassword = _hashUtility.ToSHA256(dto.Password, salt);

			return string.Compare(member.EncryptedPassword, hashPassword) == 0
				? Result.Success()
				: Result.Fail("帳密有誤");
		}
		//public Result ExistMemberLogin(string email)
		//{
		//	var memberInDb = _repo.GetMemberByEmail(email);
		//	CreateJwtToken
		//}
		public Result GoogleRegister(RegisterDTO dto)
		{
			// 判斷username是否重複
			var memberInDb = _repo.GetMemberByUsername(dto.Username);
			if (memberInDb != null) return Result.Fail("帳號重複");

			// 因在GoogleLogin()時就已經檢查過email了，故不再檢查

			// 填入剩餘欄位的值
			var salt = _hashUtility.GetSalt();
			dto.EncryptedPassword = _hashUtility.ToSHA256(dto.Password, salt);

			dto.IsConfirmed = true;
			dto.ConfirmCode = null;

			// 新增會員資料
			_repo.PostMember(dto);

			return Result.Success();
		}
		public string? CreateJwtToken(string username)
		{
			var memberInDb = _repo.GetMemberByUsername(username);
			var memberId=  memberInDb.Id;
			// 設定使用者資訊
			var claims = new List<Claim>
			{
				new Claim("username",username),
				new Claim("memberId",memberId.ToString())
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
				expires: DateTime.Now.AddSeconds(30),
				signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
			);

			// 產生JWT Token
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
		public Result Register(RegisterDTO dto)
		{
			// 判斷username是否重複
			var memberInDb = _repo.GetMemberByUsername(dto.Username);
			if (memberInDb != null) return Result.Fail("帳號重複");

			// 判斷email是否重複
			if (_repo.IsEmailExist(dto.Email)) return Result.Fail("信箱已註冊");

			// 填入剩餘欄位的值
			var salt = _hashUtility.GetSalt();
			dto.EncryptedPassword = _hashUtility.ToSHA256(dto.Password, salt);

			dto.IsConfirmed = false;

			var confirmCode = Guid.NewGuid().ToString("N");
			dto.ConfirmCode = confirmCode;

			// 新增會員資料
			_repo.PostMember(dto);

			// 寄驗證信
			var memberId = _repo.GetMemberIdByUsername(dto.Username);
			//var confirmLink = _urlHelper.Action("ActiveRegister", "Members", new { IdToString, confirmCode });
			var confirmUrl = $"http://127.0.0.1:5501/RegisterActive.html?memberId={memberId}&confirmCode={confirmCode}";
			_emailHelper.SendConfirmRegisterEmail(dto.Email, confirmUrl, dto.Username);

			return Result.Success();

		}
		public Result ActiveRegister(ActiveRegisterDTO dto)
		{
			var memberInDb = _repo.GetMemberById(dto.MemberId);
			if (memberInDb == null || memberInDb.ConfirmCode != dto.ConfirmCode) return Result.Fail("驗證錯誤");

			memberInDb.IsConfirmed = true;
			memberInDb.ConfirmCode = null;
			_repo.SaveChanges();

			return Result.Success();
		}
		public Result ProccessResetPassword(ForgotPasswordDTO dto)
		{
			// 檢查該帳號和Email是否正確
			var memberInDb = _repo.GetMemberByUsername(dto.Username);
			if (memberInDb == null || string.Compare(dto.Email, memberInDb.Email, StringComparison.CurrentCultureIgnoreCase) != 0) return Result.Fail("帳號或Email錯誤");

			// 已啟用的帳號才能重設密碼
			if (memberInDb.IsConfirmed == false) return Result.Fail("尚未啟用本帳號，請先完成才能重設密碼");

			// 填入 confirmCode，更新資料
			var confirmCode = Guid.NewGuid().ToString("N");
			memberInDb.ConfirmCode = confirmCode;
			_repo.SaveChanges();

			// 寄驗證信
			var memberId = _repo.GetMemberIdByUsername(dto.Username);
			var confirmUrl = $"http://127.0.0.1:5501/ResetPassword.html?memberId={memberId}&confirmCode={confirmCode}";
			_emailHelper.SendForgotPasswordEmail(dto.Email, confirmUrl, dto.Username);

			return Result.Success();
		}

		public Result ProccessChangePassword(ResetPasswordDTO dto)
		{
			// 檢查該帳號是否存在
			var memberInDb = _repo.GetMemberById(dto.MemberId);
			if (memberInDb == null || memberInDb.ConfirmCode != dto.ConfirmCode) return Result.Fail("找不到對應的會員紀錄");

			// 更新密碼，並將ConfirmCode清空
			var salt = _hashUtility.GetSalt();
			var hashPassword = _hashUtility.ToSHA256(dto.NewPassword, salt);

			memberInDb.EncryptedPassword = hashPassword;
			memberInDb.ConfirmCode = null;

			_repo.SaveChanges();

			return Result.Success();

		}

		public Result EditPassword(EditPasswordDTO dto)
		{
			var salt = _hashUtility.GetSalt();
			var hashOriginalPassword = _hashUtility.ToSHA256(dto.OriginalPassword, salt);

			var memberInDb = _repo.GetMemberById(dto.MemberId);
			if (memberInDb == null || memberInDb.EncryptedPassword!= hashOriginalPassword) return Result.Fail("找不到要修改的會員紀錄");

			// 更新密碼
			memberInDb.EncryptedPassword = _hashUtility.ToSHA256(dto.NewPassword, salt);

			_repo.SaveChanges();

			return Result.Success();
		}

	}
}

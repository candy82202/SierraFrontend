using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop.Infrastructure;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Repository.EFRepository;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public Result Register(RegisterDTO dto)
        {
            // 判斷username是否重複
            var memberInDb = _repo.GetMemberByUsername(dto.Username);
            if (memberInDb != null) return Result.Fail("帳號重複");

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
            _emailHelper.SendVerificationEmail(dto.Email, memberId, confirmCode);

            return Result.Success();

        }

        public Result ActiveRegister(ActiveRegisterDTO dto)
        {
            var memberInDb = _repo.GetMemberById(dto.MemberId);
            if (memberInDb == null || memberInDb.ConfirmCode != dto.ConfirmCode) return Result.Fail("驗證錯誤");

            memberInDb.IsConfirmed = true;
            memberInDb.ConfirmCode = null;
            _repo.ActiveRegister(memberInDb);
            

            return Result.Success();
            
        }

        public string? CreateJwtToken(string username)
        {
            var memberId = _repo.GetMemberIdByUsername(username).ToString();
			// 設定使用者資訊
			var claims = new List<Claim>
			{
                new Claim("username",username),
				new Claim("memberId",memberId)
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
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Repository.EFRepository;

namespace SIERRA_Server.Models.Services
{
	public class MemberService
	{
		private readonly MemberEFRepository _repo;
		private readonly HashUtility _hashUtility;
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
			dto.ConfirmCode = Guid.NewGuid().ToString("N");

			// 新增會員資料
			_repo.PostMember(dto);

			return Result.Success();

		}
	}
}

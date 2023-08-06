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

        public Result ValidLogin(LoginDTO dto)
        {
            var member = _repo.GetMemberByUsername(dto);
            if (member == null) return Result.Fail("帳密有誤");

            var salt = _hashUtility.GetSalt();
            var hashPassword = _hashUtility.ToSHA256(dto.Password, salt);

            return string.Compare(member.EncryptedPassword, hashPassword) == 0
                ? Result.Success()
                : Result.Fail("帳密有誤");
        }
    }
}

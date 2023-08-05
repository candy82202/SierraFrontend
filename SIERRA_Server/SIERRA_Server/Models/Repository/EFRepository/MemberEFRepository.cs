using Humanizer;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class MemberEFRepository : IMemberRepository
    {
        private readonly AppDbContext _db;
        public MemberEFRepository(AppDbContext db)
        {
            _db = db;
        }

        public Member? GetMemberByUsername(string username)
        {
            return _db.Members.FirstOrDefault(m => m.Username == username);
        }

        public void PostMember(RegisterDTO dto)
        {
            Member member = new Member
            {
                Username = dto.Username,
                Email = dto.Email,
                EncryptedPassword = dto.EncryptedPassword,
                IsConfirmed = dto.IsConfirmed,
                ConfirmCode = dto.ConfirmCode,
            };

            _db.Members.Add(member);
            _db.SaveChanges();
        }
    }
}

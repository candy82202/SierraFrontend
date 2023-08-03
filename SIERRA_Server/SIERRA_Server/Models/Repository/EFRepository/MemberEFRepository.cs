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

        public Member? GetMemberByUsername(LoginDTO dto)
        {
            return _db.Members.FirstOrDefault(m => m.MemberName == dto.Username);
        }
    }
}

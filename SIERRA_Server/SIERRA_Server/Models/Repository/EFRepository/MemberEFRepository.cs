using Humanizer;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;
using System.Diagnostics.Eventing.Reader;

namespace SIERRA_Server.Models.Repository.EFRepository
{
	public class MemberEFRepository : IMemberRepository
	{
		private readonly AppDbContext _db;
		public MemberEFRepository(AppDbContext db)
		{
			_db = db;
		}

		public Member GetMemberByUsername(string username)
		{
			return _db.Members.FirstOrDefault(m => m.Username == username);
		}
		public Member GetMemberById(int memberId)
		{
			return _db.Members.FirstOrDefault(m => m.Id == memberId);
		}
		public async Task<Member> GetMemberByIdAsync(int memberId)
		{
			return await _db.Members.FirstOrDefaultAsync(m => m.Id == memberId);
		}

		public Member GetMemberByEmail(string email)
		{
			return _db.Members.FirstOrDefault(m => m.Email == email);
		}

		public int GetMemberIdByUsername(string username)
		{
			var member = _db.Members.FirstOrDefault(m => m.Username == username);
			return (member != null) ? member.Id : -1;
		}

		public bool IsEmailExist(string email)
		{
			return _db.Members.Any(m => m.Email == email);
		}
		public bool IsAccountExist(string username)
		{
			return _db.Members.Any(m => m.Username == username);
		}

		public bool IsAccountExist(int memberId)
		{
			return _db.Members.Any(m => m.Id == memberId);
		}

		public void PostMember(RegisterDTO dto)
		{
			Member member = new Member
			{
				Username = dto.Username,
				Email = dto.Email,
				EncryptedPassword = dto.EncryptedPassword,
				ImageName = dto.ImageName,
				IsConfirmed = dto.IsConfirmed,
				ConfirmCode = dto.ConfirmCode,
			};

			_db.Members.Add(member);
			_db.SaveChanges();
		}

		public void SaveChanges()
		{
			_db.SaveChanges();
		}

		public async Task SaveChangesAsync()
		{
			await _db.SaveChangesAsync();
		}
	}
}

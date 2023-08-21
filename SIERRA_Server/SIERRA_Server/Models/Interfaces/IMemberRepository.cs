using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;

namespace SIERRA_Server.Models.Interfaces
{
	public interface IMemberRepository
	{
		Member? GetMemberByUsername(string username);
		Member? GetMemberById(int memberId);
		Task<Member> GetMemberByIdAsync(int memberId);
		Member? GetMemberByEmail(string email);
		int GetMemberIdByUsername(string username);
		bool IsEmailExist(string email);
		bool IsAccountExist(string username);
		bool IsAccountExist(int memberId);
		void PostMember(RegisterDTO dto);
		void SaveChanges();
		Task SaveChangesAsync();

	}
}

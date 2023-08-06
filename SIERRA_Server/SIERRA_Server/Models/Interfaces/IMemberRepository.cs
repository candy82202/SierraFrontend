using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IMemberRepository
    {
        Member? GetMemberByUsername(string username);
        bool isAccountExist(string username);
        void PostMember(RegisterDTO dto);


	}
}

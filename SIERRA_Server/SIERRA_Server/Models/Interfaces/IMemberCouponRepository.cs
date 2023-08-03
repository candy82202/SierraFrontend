using SIERRA_Server.Models.DTOs.Peomotions;
using SIERRA_Server.Models.Infra;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IMemberCouponRepository
	{
	    Task<IEnumerable<MemberCouponDto>>GetUsableCoupon(int? MemberId);
		Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? MemberId);
        Task<bool>CheckCouponExist(string code);
        Task<ResultForCheck> CheckHaveSame(int memberId, string code);
        Task<string> GetCouponByCode(MemberCouponCreateDto dto);
    }
}

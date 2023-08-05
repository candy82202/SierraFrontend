using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IMemberCouponRepository
	{
	    Task<IEnumerable<MemberCoupon>>GetUsableCoupon(int? MemberId);
		Task<IEnumerable<MemberCoupon>> GetCouponCanNotUseNow(int? MemberId);
        Task<bool>CheckCouponExist(string code);
        Task<ResultForCheck> CheckHaveSame(int memberId, string code);
        Task<string> GetCouponByCode(MemberCouponCreateDto dto);
        Task<IEnumerable<MemberCouponHasUsedDto>> GetCouponHasUsed(int? memberId);
        Task<DessertCart> GetDessertCart(int memberId);

    }
}

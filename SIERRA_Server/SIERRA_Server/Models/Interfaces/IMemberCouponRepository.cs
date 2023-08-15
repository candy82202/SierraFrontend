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
        Task<IEnumerable<Coupon>> GetPromotionCoupons();
        Task<IEnumerable<int>> GetAllMemberPromotionCoupon(int memberId);
		bool IsMemberExist(int memberId);
		bool IsMemberCouponExist(int memberCouponId);
		bool IsThisMemberHaveThisCoupon(int memberId, int memberCouponId);
		Task<Coupon> GetMemberCouponById(int memberCouponId);
        bool HasCouponBeenUsed(int memberCouponId);
		bool IsPromotionCouponAndReady(int memberCouponId);
		void RecordCouponInCart(DessertCart cart, int memberCouponId);
		Task<IEnumerable<CouponSetting>> GetPrizes();
		Task<bool> HasPlayedGame(int memberId);
		Task<string> AddCouponAndRecordMemberPlay(int memberId, int resultCouponId);
	}
}

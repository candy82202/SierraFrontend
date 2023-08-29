using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra.Promotions;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IMemberCouponRepository
	{
	    Task<IEnumerable<MemberCoupon>>GetUsableCoupon(int? MemberId);
		Task<IEnumerable<MemberCoupon>> GetCouponCanNotUseNow(int? MemberId);
        Task<bool>CheckCouponExist(string code);
        Task<ResultForCheck> CheckHaveSame(int memberId, string code);
        Task<AddCouponResult> GetCouponByCode(MemberCouponCreateDto dto);
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
		void RecordCouponInCart(DessertCart cart, int memberCouponId, int result);
		Task<IEnumerable<CouponSetting>> GetPrizes();
		Task<bool> HasPlayedGame(int memberId);
		Task<string> AddCouponAndRecordMemberPlay(int memberId, int resultCouponId);
        Task<Coupon> FindCoupon(int couponId);
		Task<CouponSetting[]> GetWeeklyGameCouponSettings();
        Task<bool> HasPlayedWeeklyGame(int memberId);
        Task AddCouponAndRecordMemberPlayWeeklyGame(int memberId, Coupon coupon);
		Task<IEnumerable<DiscountGroupItem>> FindSuggestProduct(int discountGroupId);
        Task<bool> CancelUsingCoupon(int memberId);
        Task<object?> GetUsingCoupon(int memberId);
		void LetMembersCanPlayDailyGame();
        void LetMembersCanPlayWeeklyGame();
        IEnumerable<Member> GetBirthdayMemberInThisMonth();
        Coupon GetBirthdayCoupon();
        void AddBirthdayCoupons(IEnumerable<MemberCoupon> memberCoupons);
    }
}

using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class MemberCouponAndCouponDetailDto
    {
        public int MemberCouponId { get; set; }
        public int CouponId { get; set; }
        public int? DiscountGroupId { get; set; }
        public string CouponName { get; set; }
		public int? LimitType { get; set; }
		public int? LimitValue { get; set; }
		public virtual DiscountGroup DiscountGroup { get; set; }
	}
}
using SIERRA_Server.Models.DTOs;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Exts
{
	public static class MemberCouponExts
	{
		public static MemberCouponDto ToMemberCouponDto(this MemberCoupon entity)
		{
			var dessertNames =entity.Coupon==null||entity.Coupon.DiscountGroup == null ? "所有商品" : entity.Coupon.DiscountGroup.DiscountGroupItems.Count() > 0 ? string.Join(",", entity.Coupon.DiscountGroup.DiscountGroupItems.Select(dgi => dgi.Dessert.DessertName)) : "無";
			int? itemsCount = entity.Coupon.DiscountGroup==null?null:entity.Coupon.DiscountGroup.DiscountGroupItems.Count();
			string applyTo;
			if (itemsCount == null)
			{
				applyTo = dessertNames;
			}
			else
			{
				applyTo = $"{dessertNames},共{itemsCount}項";
			}
			return new MemberCouponDto()
			{
				MemberCouponId = entity.MemberCouponId,
				CouponName = entity.CouponName,
				ExpireAt = entity.ExpireAt,
				ApplyTo = applyTo
			};
		}
		public static MemberCouponCanNotUseDto ToMemberCouponCanNotUseDto(this MemberCoupon entity)
		{
			var dessertNames = entity.Coupon == null || entity.Coupon.DiscountGroup == null ? "所有商品" : entity.Coupon.DiscountGroup.DiscountGroupItems.Count() > 0 ? string.Join(",", entity.Coupon.DiscountGroup.DiscountGroupItems.Select(dgi => dgi.Dessert.DessertName)) : "無";
			int? itemsCount = entity.Coupon.DiscountGroup == null ? null : entity.Coupon.DiscountGroup.DiscountGroupItems.Count();
			string applyTo;
			if (itemsCount == null)
			{
				applyTo = dessertNames;
			}
			else
			{
				applyTo = $"{dessertNames},共{itemsCount}項";
			}
			return new MemberCouponCanNotUseDto()
			{
				MemberCouponId = entity.MemberCouponId,
				CouponName = entity.CouponName,
				StartAt = (DateTime)entity.Coupon.StartAt,
				ApplyTo = applyTo
			};
		}
	}
}

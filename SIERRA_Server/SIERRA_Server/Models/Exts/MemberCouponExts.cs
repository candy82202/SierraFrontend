using SIERRA_Server.Models.DTOs.Promotions;
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
				ApplyTo = applyTo,
				CouponType = entity.Coupon.DiscountType
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
				ApplyTo = applyTo,
				CouponType=entity.Coupon.DiscountType
			};
		}
        public static MemberCouponHasUsedDto ToMemberCouponHasUsedDto(this MemberCoupon entity)
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
			return new MemberCouponHasUsedDto()
			{
				MemberCouponId = entity.MemberCouponId,
				CouponName = entity.CouponName,
				UsedAt = (DateTime)entity.UseAt,
				ApplyTo = applyTo,
				CouponType = entity.Coupon.DiscountType
				
			};
		}
		public static MemberCouponAndCouponDetailDto ToMemberCouponAndCouponDetailDto(this MemberCoupon entity)
		{
			return new MemberCouponAndCouponDetailDto()
			{
				MemberCouponId = entity.MemberCouponId,
				CouponName = entity.CouponName,
				CouponId = entity.CouponId,
				DiscountGroup = entity.Coupon.DiscountGroup,
				DiscountGroupId = entity.Coupon.DiscountGroupId,
				LimitType = entity.Coupon.LimitType,
				LimitValue = entity.Coupon.LimitValue,
			};
		}
        public static CouponCanGetDto ToCouponCanGetDto(this Coupon entity)
        {
            var dessertNames =  entity.DiscountGroup == null ? "所有商品" : entity.DiscountGroup.DiscountGroupItems.Count() > 0 ? string.Join(",", entity.DiscountGroup.DiscountGroupItems.Select(dgi => dgi.Dessert.DessertName)) : "無";
            int? itemsCount = entity.DiscountGroup == null ? null : entity.DiscountGroup.DiscountGroupItems.Count();
            string applyTo;
            if (itemsCount == null)
            {
                applyTo = dessertNames;
            }
            else
            {
                applyTo = $"{dessertNames},共{itemsCount}項";
            }
            return new CouponCanGetDto()
            {
                CouponName = entity.CouponName,
                ExpireAt = ((DateTime)entity.EndAt).ToString("yyyy-MM-dd"),
                ApplyTo = applyTo,
            };
        }

		public static MemberCouponCanUseDto ToMemberCouponCanUseDto(this MemberCoupon entity)
		{
			var dessertNames = entity.Coupon == null || entity.Coupon.DiscountGroup == null ? "所有商品" : entity.Coupon.DiscountGroup.DiscountGroupItems.Count() > 0 ? string.Join(",", entity.Coupon.DiscountGroup.DiscountGroupItems.Select(dgi => dgi.Dessert.DessertName)) : "無";
			int? itemsCount = entity.Coupon.DiscountGroup == null ? null : entity.Coupon.DiscountGroup.DiscountGroupItems.Count();
			string applyTo;
			var discountGrroupName = entity.Coupon.DiscountGroup?.DiscountGroupName;
			if (itemsCount == null)
			{
				applyTo = dessertNames;
			}
			else
			{
				applyTo = $"{discountGrroupName},共{itemsCount}項";
			}
			var startAt = entity.Coupon.CouponCategoryId == 2 ? entity.Coupon.StartAt : entity.CreateAt;
			return new MemberCouponCanUseDto()
			{
				MemberCouponId = entity.MemberCouponId,
				CouponName = entity.CouponName,
				CreateAt = entity.CreateAt,
				ExpireAt = entity.ExpireAt,
				StartAt = (DateTime)startAt,
				ApplyTo = applyTo,
				ApplyToDeTail = dessertNames,
				CouponType = entity.Coupon.DiscountType
			};
		}
	}
}

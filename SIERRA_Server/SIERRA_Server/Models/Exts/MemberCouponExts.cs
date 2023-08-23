using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Infra.Promotions;

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
            var discountGrroupName = entity.DiscountGroup?.DiscountGroupName;
            if (itemsCount == null)
            {
                applyTo = dessertNames;
            }
            else
            {
                applyTo = $"{discountGrroupName},共{itemsCount}項";
            }
            return new CouponCanGetDto()
            {
                CouponName = entity.CouponName,
                ExpireAt = ((DateTime)entity.EndAt).ToString("yyyy-MM-dd"),
                ApplyTo = applyTo,
				ApplyToDetail = dessertNames
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
				CouponType = entity.Coupon.DiscountType,
			};
		}
		public static SuggestProductDto ToSuggestProductDto(this DiscountGroupItem entity)
		{
			return new SuggestProductDto()
			{
				DessertId = entity.DessertId,
				DessertName = entity.Dessert.DessertName,
				DessertImage = entity.Dessert.DessertImages.First().DessertImageName,
				UnitPrice = entity.Dessert.Specifications.First().UnitPrice,
			};
		}
		public static IneligibleMemberCouponDto ToIneligibleMemberCouponDto(this MemberCoupon entity,DessertCart cart)
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
			string needWhat="";
			if (entity.Coupon.LimitType == 1)
			{
				var neededCount = entity.Coupon.LimitValue;
				var count = cart.DessertCartItems.Where(dci => dci.Dessert.DiscountGroupItems.Any(dgi => dgi.DiscountGroupId == entity.Coupon.DiscountGroupId))
																	  .Sum(dci => dci.Quantity);
				needWhat = $"還差{neededCount - count}件 {entity.Coupon.DiscountGroup.DiscountGroupName} 商品";
			}
			else if(entity.Coupon.LimitType == 2)
			{
				var neededCost = entity.Coupon.LimitValue;
				var totalPrice = cart.DessertCartItems.Select(dci => dci.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
												  ? Math.Round((decimal)(dci.Specification.UnitPrice) * ((dci.Dessert.Discounts.First().DiscountPrice) / 100), 0, MidpointRounding.AwayFromZero)
												  : dci.Specification.UnitPrice)
												  .Sum();
				needWhat = $"還差{neededCost-totalPrice}元";
			}
			else
			{
				needWhat = $"還差1件 {entity.Coupon.DiscountGroup.DiscountGroupName} 商品";
			}

			return new IneligibleMemberCouponDto()
			{
				MemberCouponId = entity.MemberCouponId,
				CouponName = entity.CouponName,
				CreateAt = entity.CreateAt,
				ExpireAt = entity.ExpireAt,
				StartAt = (DateTime)startAt,
				ApplyTo = applyTo,
				ApplyToDeTail = dessertNames,
				CouponType = entity.Coupon.DiscountType,
				NeedWhat = needWhat,
			};
		}
	}
}

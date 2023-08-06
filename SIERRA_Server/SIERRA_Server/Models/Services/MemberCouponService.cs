using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Exts;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Services
{
    public class MemberCouponService
	{
		private IMemberCouponRepository _repo;

        public MemberCouponService(IMemberCouponRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MemberCouponDto>> GetUsableCoupon(int? memberId)
        {
            var coupons = await _repo.GetUsableCoupon(memberId);
            return coupons.Select(mc => mc.ToMemberCouponDto());
        }

        public async Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? memberId)
        {
            var coupons = await _repo.GetCouponCanNotUseNow(memberId);
            return coupons.Select(mc => mc.ToMemberCouponCanNotUseDto());
        }

        public async Task<string> GetCouponByCode(int memberId, string code)
        {
            var isExist = await _repo.CheckCouponExist(code);
            if (!isExist)
            {
                return "查無此優惠碼";
            }
            else
            {
                var result =await _repo.CheckHaveSame(memberId, code);
                if (result.HaveSame)
                {
                    return "已領取過此優惠券";
                }
                else
                {
                    MemberCouponCreateDto dto = new MemberCouponCreateDto();
                    dto.CouponId = result.CouponId;
                    dto.MemberId = memberId;
                    return await _repo.GetCouponByCode(dto);
                }
            }
            
        }

        public async Task<IEnumerable<MemberCouponHasUsedDto>> GetCouponHasUsed(int? memberId)
        {
            var coupons = await _repo.GetCouponHasUsed(memberId);
            return coupons;
        }

        public async Task<IEnumerable<MemberCouponDto>> GetCouponMeetCriteria(int memberId)
        {
            var coupons = await DoThisToGetCouponMeetCriteria(memberId);
            var result = coupons.Select(c => c.ToMemberCouponDto());
            return result;
        }

        public async Task<IEnumerable<MemberCouponDto>> GetIneligibleCoupon(int memberId)
        {
            var coupons = await _repo.GetUsableCoupon(memberId);
            var usableCoupons = await DoThisToGetCouponMeetCriteria(memberId);
            var ineligibleCoupons = coupons.Except(usableCoupons).Select(mc => mc.ToMemberCouponDto());
            return ineligibleCoupons;
        }

        private async Task<IEnumerable<MemberCoupon>> DoThisToGetCouponMeetCriteria(int memberId)
        {
            var coupons = await _repo.GetUsableCoupon(memberId);
            //先把一定可以用的優惠券加進來
            var couponsMeetCriteria = coupons.Where(mc => mc.Coupon.DiscountGroupId == null && mc.Coupon.LimitType == null).ToList();
            //列出需要檢查的
            var waitToCheck = coupons.Except(couponsMeetCriteria);
            //找出此會員的購物車
            var cart = await _repo.GetDessertCart(memberId);
            //算出總金額
            var totalPrice = cart.DessertCartItems.Select(dci => dci.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
                                                  ? Math.Round((decimal)(dci.Specification.UnitPrice) * ((dci.Dessert.Discounts.First().DiscountPrice) / 100), 0, MidpointRounding.AwayFromZero)
                                                  : dci.Specification.UnitPrice)
                                                  .Sum();
            //列出購物車所優商品的id
            var cartItemsDessertIds = cart.DessertCartItems.Select(dci => dci.DessertId);
            //列出id跟數量
            var dessertIdAndQtys = cart.DessertCartItems.Select(dci => new {
                Id = dci.DessertId,
                Qty = dci.Quantity
            }).ToArray();
            //檢查剩下的優惠券
            foreach (MemberCoupon coupon in waitToCheck)
            {
                if (coupon.Coupon.DiscountGroupId == null)
                {
                    if (totalPrice >= coupon.Coupon.LimitValue)
                    {
                        couponsMeetCriteria.Add(coupon);
                    }
                    else continue;
                }
                else
                {
                    var discountGroupDessertIds = coupon.Coupon.DiscountGroup.DiscountGroupItems.Select(dgi => dgi.DessertId);

                    if (coupon.Coupon.LimitType == null)
                    {
                        var commonItems = cartItemsDessertIds.Intersect(discountGroupDessertIds);
                        if (commonItems.Any())
                        {
                            couponsMeetCriteria.Add(coupon);
                        }
                        else continue;
                        //這種方法執行效率較佳但上面的比較簡潔
                        //for (int i = 0; i < discountGroupDessertIds.Length; i++)
                        //{
                        //    if (cartItemsDessertIds.Contains(discountGroupDessertIds[i]))
                        //    {
                        //        couponsMeetCriteria.Add(coupon);
                        //        break;
                        //    }
                        //    else continue;
                        //}
                    }
                    else
                    {
                        var neededCount = coupon.Coupon.LimitValue;
                        var buyCount = 0;
                        for (int i = 0; i < dessertIdAndQtys.Length; i++)
                        {
                            if (buyCount >= neededCount)
                            {
                                couponsMeetCriteria.Add(coupon);
                                break;
                            }
                            else
                            {
                                if (discountGroupDessertIds.Contains(dessertIdAndQtys[i].Id))
                                {
                                    buyCount += dessertIdAndQtys[i].Qty;
                                }
                                else continue;
                            }
                        }
                    }
                }
            }
            return couponsMeetCriteria;
        }
    }
}

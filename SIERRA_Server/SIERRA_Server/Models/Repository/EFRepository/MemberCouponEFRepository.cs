using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Exts;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class MemberCouponEFRepository : IMemberCouponRepository
	{
		private readonly AppDbContext _db;
        public MemberCouponEFRepository(AppDbContext db)
        {
            _db= db;
        }
        public async Task<IEnumerable<MemberCouponDto>> GetUsableCoupon(int? MemberId)
		{
			var coupons = await _db.MemberCoupons.Include(mc => mc.Coupon)
										   .ThenInclude(c => c.DiscountGroup)
										   .ThenInclude(dg => dg.DiscountGroupItems)
										   .ThenInclude(dgi => dgi.Dessert)
										   .Where(mc => mc.MemberId == MemberId)
										   .Where(mc => mc.UseAt == null && mc.ExpireAt > DateTime.Now)
										   .Where(mc => mc.Coupon.StartAt == null || mc.Coupon.StartAt <= DateTime.Now)
                                           .OrderBy(mc=>mc.CreateAt)
										   .Select(mc => mc.ToMemberCouponDto())
										   .ToListAsync();
			return coupons;
		}

        public async Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? MemberId)
        {

            var coupons = await _db.MemberCoupons.Include(mc => mc.Coupon)
                                           .ThenInclude(c => c.DiscountGroup)
                                           .ThenInclude(dg => dg.DiscountGroupItems)
                                           .ThenInclude(dgi => dgi.Dessert)
                                           .Where(mc => mc.MemberId == MemberId)
                                           .Where(mc => mc.UseAt == null && mc.ExpireAt > DateTime.Now)
                                           .Where(mc => mc.Coupon.CouponCategoryId == 2 && mc.Coupon.StartAt > DateTime.Now)
                                           .OrderBy(mc => mc.CreateAt)
                                           .Select(mc => mc.ToMemberCouponCanNotUseDto())
                                           .ToListAsync();
            return coupons;
        }

        public async Task<bool>  CheckCouponExist(string code)
        {
            if(await _db.Coupons.Where(c=>c.CouponCategoryId==4).AnyAsync(c=>c.CouponCode==code))
            {
                return  true;
            }
            else return false;
        }

        public async Task<ResultForCheck>CheckHaveSame(int memberId, string code)
        {
            var coupon =await _db.Coupons.Where(c=>c.CouponCategoryId==4).FirstAsync(c=>c.CouponCode==code);
            var couponId = coupon.CouponId;
            bool haveSame = await _db.MemberCoupons.Where(mc=>mc.MemberId==memberId)
                                                   .AnyAsync(c=>c.CouponId == couponId);
            return new ResultForCheck(couponId,haveSame);

        }

        public async Task<string> GetCouponByCode(MemberCouponCreateDto dto)
        {
            var coupon = _db.Coupons.Find(dto.CouponId);
            if(coupon == null)
            {
                return "查無此優惠券";
            }
            else
            {
                var newMemberCoupon = new MemberCoupon();
                newMemberCoupon.MemberId = dto.MemberId;
                newMemberCoupon.CouponId = dto.CouponId;
                newMemberCoupon.CouponName = coupon.CouponName;
                newMemberCoupon.CreateAt = DateTime.Now;
                newMemberCoupon.ExpireAt = DateTime.Now.AddDays((double)coupon.Expiration);
                _db.MemberCoupons.Add(newMemberCoupon);
                await _db.SaveChangesAsync();
                return newMemberCoupon.CouponName;
            }
        }

        public async Task<IEnumerable<MemberCouponHasUsedDto>> GetCouponHasUsed(int? memberId)
        {
            var coupons = _db.MemberCoupons.Where(mc => mc.MemberId == memberId)
                                                .Where(mc => mc.UseAt != null && ((TimeSpan)(DateTime.Now - mc.UseAt)).TotalDays < 30)
                                                .Select(mc=>mc.ToMemberCouponHasUsedDto());
            return coupons;
        }


		public async Task<IEnumerable<MemberCouponDto>> GetCouponMeetCriteria(int memberId)
        {
            var coupons = await _db.MemberCoupons.Include(mc => mc.Coupon)
                                           .ThenInclude(c => c.DiscountGroup)
                                           .ThenInclude(dg => dg.DiscountGroupItems)
                                           .ThenInclude(dgi => dgi.Dessert)
                                           .Where(mc => mc.MemberId == memberId)
                                           .Where(mc => mc.UseAt == null && mc.ExpireAt > DateTime.Now)
                                           .Where(mc => mc.Coupon.StartAt == null || mc.Coupon.StartAt <= DateTime.Now)
                                           .ToListAsync();

            var couponsMeetCriteria = coupons.Where(mc => mc.Coupon.DiscountGroupId == null&&mc.Coupon.LimitType==null).ToList();
            var waitToCheck = coupons.Except(couponsMeetCriteria);
            //取得該會員的購物車
            var member =await _db.Members.FindAsync(memberId);
            var memberName = member.UserName;
            var cart = await _db.DessertCarts.Include(dc=>dc.DessertCartItems)
                                             .ThenInclude(dci=>dci.Specification)
                                             .ThenInclude(dci=>dci.Dessert)
                                             .ThenInclude(d=>d.Discounts)
                                             .FirstOrDefaultAsync(dc => dc.UserName == memberName);
            var totalPrice = cart.DessertCartItems.Select(dci => dci.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now) 
                                                  ? Math.Round((decimal)(dci.Specification.UnitPrice) * ((dci.Dessert.Discounts.First().DiscountPrice) / 100), 0, MidpointRounding.AwayFromZero) 
                                                  : dci.Specification.UnitPrice)
                                                  .Sum();
            var cartItemsDessertIds = cart.DessertCartItems.Select(dci => dci.DessertId);
            var dessertIdAndQtys = cart.DessertCartItems.Select(dci => new {
                Id=dci.DessertId,
                Qty = dci.Quantity
            }).ToArray();
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
                        for (int i = 0;i< dessertIdAndQtys.Length; i++)
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
            return couponsMeetCriteria.Select(c=>c.ToMemberCouponDto());
        }
    }
}

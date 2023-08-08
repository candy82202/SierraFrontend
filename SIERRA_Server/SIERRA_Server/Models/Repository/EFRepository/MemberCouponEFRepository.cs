using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Exts;
using SIERRA_Server.Models.Infra;
using SIERRA_Server.Models.Interfaces;
using System.Linq;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class MemberCouponEFRepository : IMemberCouponRepository
	{
		private readonly AppDbContext _db;
        public MemberCouponEFRepository(AppDbContext db)
        {
            _db= db;
        }
        public async Task<IEnumerable<MemberCoupon>> GetUsableCoupon(int? MemberId)
		{
			var coupons = await _db.MemberCoupons.Include(mc => mc.Coupon)
										   .ThenInclude(c => c.DiscountGroup)
										   .ThenInclude(dg => dg.DiscountGroupItems)
										   .ThenInclude(dgi => dgi.Dessert)
										   .Where(mc => mc.MemberId == MemberId)
										   .Where(mc => mc.UseAt == null && mc.ExpireAt > DateTime.Now)
										   .Where(mc => mc.Coupon.StartAt == null || mc.Coupon.StartAt <= DateTime.Now)
                                           .OrderBy(mc=>mc.CreateAt)
										   .ToListAsync();
			return coupons;
		}

        public async Task<IEnumerable<MemberCoupon>> GetCouponCanNotUseNow(int? MemberId)
        {

            var coupons = await _db.MemberCoupons.Include(mc => mc.Coupon)
                                           .ThenInclude(c => c.DiscountGroup)
                                           .ThenInclude(dg => dg.DiscountGroupItems)
                                           .ThenInclude(dgi => dgi.Dessert)
                                           .Where(mc => mc.MemberId == MemberId)
                                           .Where(mc => mc.UseAt == null && mc.ExpireAt > DateTime.Now)
                                           .Where(mc => mc.Coupon.CouponCategoryId == 2 && mc.Coupon.StartAt > DateTime.Now)
                                           .OrderBy(mc => mc.CreateAt)
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
            var coupons =await _db.MemberCoupons.Where(mc => mc.MemberId == memberId)
                                                .Where(mc => mc.UseAt != null && ((TimeSpan)(DateTime.Now - mc.UseAt)).TotalDays < 30)
                                                .Select(mc=>mc.ToMemberCouponHasUsedDto())
                                                .ToListAsync();
            return coupons;
        }
        public async Task<DessertCart> GetDessertCart(int memberId)
        {
            var member = await _db.Members.FindAsync(memberId);
            var memberName = member.Username;
            var cart = await _db.DessertCarts.Include(dc => dc.DessertCartItems)
                                             .ThenInclude(dci => dci.Specification)
                                             .ThenInclude(dci => dci.Dessert)
                                             .ThenInclude(d => d.Discounts)
                                             .FirstOrDefaultAsync(dc => dc.Username == memberName);
            return cart;
        }

        public async Task<IEnumerable<Coupon>> GetPromotionCoupons()
        {
            var coupons =await _db.Promotions.Include(p=>p.Coupon)
                                        .Where(p=>p.CouponId!=null)
                                        .Where(p=>p.LaunchAt<DateTime.Now&&p.EndAt>DateTime.Now)
                                        .Select (p=>p.Coupon)
                                        .Distinct()
                                        .ToListAsync();
            return coupons;
        }

        public async Task<IEnumerable<int>> GetAllMemberPromotionCoupon(int memberId)
        {
            var coupons = await _db.MemberCoupons.Include(mc=>mc.Coupon)
                                                 .ThenInclude(c=>c.Promotions)
                                                 .Where(mc=>mc.MemberId==memberId)
                                                 .Where(mc=>mc.Coupon.CouponCategoryId==2)
                                                 .Select(mc=>mc.CouponId)
                                                 .ToListAsync();
            return coupons;
        }
        public  bool IsMemberExist(int memberId)
        {
            var result =  _db.Members.Any(m=>m.Id==memberId);
            return result;
        }
		public  bool IsMemberCouponExist(int memberCouponId)
		{
			var result =  _db.MemberCoupons.Any(m => m.MemberCouponId == memberCouponId);
			return result;
		}
		public  bool IsThisMemberHaveThisCoupon(int memberId, int memberCouponId)
		{
            var coupon = _db.MemberCoupons.Find(memberCouponId);
            if (coupon.MemberId == memberId)
            {
                return true;
            }
            else return false;
		}
        public async Task<Coupon> GetMemberCouponById(int memberCouponId)
        {
            var coupon = await _db.MemberCoupons.Include(mc=>mc.Coupon)
                                                .ThenInclude(c=>c.DiscountGroup)
                                                .ThenInclude(d=>d.DiscountGroupItems)
                                                .ThenInclude(dgi=>dgi.Dessert)
                                                .FirstAsync(mc=>mc.MemberCouponId== memberCouponId);
            return coupon.Coupon;
        }
        public bool HasCouponBeenUsed(int memberCouponId)
        {
            var result = _db.MemberCoupons.Find(memberCouponId).UseAt==null? false: true;
            return result;
        }
        public bool IsPromotionCouponAndReady(int memberCouponId)
        {
            var coupon = _db.MemberCoupons.Include(mc=>mc.Coupon).First(mc=>mc.MemberCouponId == memberCouponId);
            if (coupon.Coupon.CouponCategoryId == 2)
            {
                if (coupon.Coupon.StartAt < DateTime.Now && coupon.Coupon.EndAt > DateTime.Now)
                {
                    return true;
                }else return false;
            }
            else
            {
                return true;
            }
                                          
        }


	}
}

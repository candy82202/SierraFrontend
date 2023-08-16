using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class PromotionEFRepository : IPromotionRepository
    {
        private readonly AppDbContext _db;
        public PromotionEFRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Promotion>> GetPromotionsNow()
        {
            var promotions =await _db.Promotions.Where(p=>p.LaunchAt<DateTime.Now&&p.EndAt>DateTime.Now)
                                                .ToListAsync();
            return promotions;
        }

        public async Task<AddCouponResult> GetPromotionCoupon(int memberId, int couponId)
        {
            var coupon = await _db.Coupons.FindAsync(couponId);
            var memberCoupon = new MemberCoupon()
            {
                MemberId = memberId,
                CouponId = couponId,
                CouponName = coupon.CouponName,
                CreateAt = DateTime.Now,
                ExpireAt = (DateTime)coupon.EndAt
            };
            _db.MemberCoupons.Add(memberCoupon);
            await _db.SaveChangesAsync();
            return AddCouponResult.Success(memberCoupon.CouponName);
        }

        public async Task<bool> HasGottenCoupon(int memberId, int couponId)
        {
            var memberCoupons = await _db.MemberCoupons.Where(mc => mc.MemberId == memberId)
                                                       .ToListAsync();
            var sameCoupon = memberCoupons.FirstOrDefault(mc=>mc.CouponId==couponId);
            if (sameCoupon == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsPromotionCoupon(int couponId)
        {
            var coupon =await _db.Coupons.Include(c=>c.Promotions)
                                         .FirstOrDefaultAsync(c=>c.CouponId == couponId);
            if(coupon==null || coupon.CouponCategoryId!=2 ||coupon.Promotions.First().LaunchAt>DateTime.Now || coupon.Promotions.First().EndAt < DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

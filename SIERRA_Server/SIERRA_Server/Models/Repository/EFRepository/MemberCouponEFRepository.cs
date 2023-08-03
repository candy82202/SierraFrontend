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
                                                .AsEnumerable()
                                                .Where(mc => mc.UseAt != null && ((TimeSpan)(DateTime.Now - mc.UseAt)).TotalDays < 30)
                                                .Select(mc=>mc.ToMemberCouponHasUsedDto());
            return coupons;
        }
    }
}

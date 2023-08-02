using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Exts;
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
										   .Select(mc => mc.ToMemberCouponDto())
										   .ToListAsync();
			return coupons;
		}
	}
}

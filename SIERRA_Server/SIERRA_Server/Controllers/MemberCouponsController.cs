using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Exts;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class MemberCouponsController : ControllerBase
	{
		private readonly AppDbContext _db;
		public MemberCouponsController(AppDbContext db)
		{
			_db = db;
		}
		[HttpPost]
		public async Task<IEnumerable<MemberCouponDto>> GetUsableCoupon(int? MemberId)
		{
			if(MemberId == null ||_db.Members.Find(MemberId)==null)
			{
				return Enumerable.Empty<MemberCouponDto>();
			}
			var coupons = await _db.MemberCoupons.Include(mc=>mc.Coupon)
										   .ThenInclude(c=>c.DiscountGroup)
										   .ThenInclude(dg=>dg.DiscountGroupItems)
										   .ThenInclude(dgi=>dgi.Dessert)
										   .Where(mc=>mc.MemberId== MemberId)
										   .Where(mc=>mc.UseAt==null&&mc.ExpireAt>DateTime.Now)
										   .Where(mc=>mc.Coupon.StartAt==null||mc.Coupon.StartAt<=DateTime.Now)
										   .Select(mc=>mc.ToMemberCouponDto()).ToListAsync();
			return coupons;
		}
		[HttpPost("CanNotUse")]
		public async Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? MemberId)
		{
			if (MemberId == null || _db.Members.Find(MemberId) == null)
			{
				return Enumerable.Empty<MemberCouponCanNotUseDto>();
			}
			var coupons = await _db.MemberCoupons.Include(mc => mc.Coupon)
										   .ThenInclude(c => c.DiscountGroup)
										   .ThenInclude(dg => dg.DiscountGroupItems)
										   .ThenInclude(dgi => dgi.Dessert)
										   .Where(mc => mc.MemberId == MemberId)
										   .Where(mc => mc.UseAt == null && mc.ExpireAt > DateTime.Now)
										   .Where(mc=>mc.Coupon.CouponCategoryId==2&&mc.Coupon.StartAt>DateTime.Now)
										   .Select(mc => mc.ToMemberCouponCanNotUseDto()).ToListAsync();
			return coupons;
		}
	}
}

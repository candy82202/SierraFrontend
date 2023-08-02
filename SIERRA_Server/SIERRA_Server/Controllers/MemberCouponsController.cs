using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Exts;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Services;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class MemberCouponsController : ControllerBase
	{
		private IMemberCouponRepository _repo;
        public MemberCouponsController(IMemberCouponRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
		public async Task<IEnumerable<MemberCouponDto>> GetUsableCoupon(int? MemberId)
		{
			if(MemberId == null)
			{
				return Enumerable.Empty<MemberCouponDto>();
			}
			var server = new MemberCouponService(_repo);
			var coupons =await server.GetUsableCoupon(MemberId);
			return coupons;
		}
		//[HttpPost("CanNotUse")]
		//public async Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? MemberId)
		//{
		//	if (MemberId == null || _db.Members.Find(MemberId) == null)
		//	{
		//		return Enumerable.Empty<MemberCouponCanNotUseDto>();
		//	}
		//	var coupons = await _db.MemberCoupons.Include(mc => mc.Coupon)
		//								   .ThenInclude(c => c.DiscountGroup)
		//								   .ThenInclude(dg => dg.DiscountGroupItems)
		//								   .ThenInclude(dgi => dgi.Dessert)
		//								   .Where(mc => mc.MemberId == MemberId)
		//								   .Where(mc => mc.UseAt == null && mc.ExpireAt > DateTime.Now)
		//								   .Where(mc=>mc.Coupon.CouponCategoryId==2&&mc.Coupon.StartAt>DateTime.Now)
		//								   .Select(mc => mc.ToMemberCouponCanNotUseDto()).ToListAsync();
		//	return coupons;
		//}
	}
}

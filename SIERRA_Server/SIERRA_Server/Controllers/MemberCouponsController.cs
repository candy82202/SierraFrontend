using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Promotions;
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
        [HttpGet]
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
		[HttpGet("CanNotUse")]
		public async Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? MemberId)
		{
			if (MemberId == null)
			{
				return Enumerable.Empty<MemberCouponCanNotUseDto>();
			}
			var server = new MemberCouponService(_repo);
			var coupons = await server.GetCouponCanNotUseNow(MemberId);
			return coupons;
		}
		[HttpGet("GetUsed")]
		public async Task<IEnumerable<MemberCouponHasUsedDto>> GetCouponHasUsed(int? MemberId)
		{
            if (MemberId == null)
            {
                return Enumerable.Empty<MemberCouponHasUsedDto>();
            }
			var server = new MemberCouponService(_repo);
			var coupons = await server.GetCouponHasUsed(MemberId); 
			return coupons;
        }
		[HttpPost]
		public async Task<string> GetCouponByCode(int? MemberId,string? code)
		{
			if(MemberId == null ||string.IsNullOrEmpty(code))
			{
				return "查無此優惠碼";
			}
			var server = new MemberCouponService(_repo);
			return await server.GetCouponByCode((int)MemberId, code);
		}

		[HttpGet("MeetCriteria")]
		public async Task<IEnumerable<MemberCouponDto>> GetCouponMeetCriteria(int? MemberId)
		{
			if (MemberId == null)
			{
				return Enumerable.Empty<MemberCouponDto>();
			}
			var server = new MemberCouponService(_repo);
			return await server.GetCouponMeetCriteria((int)MemberId);
        }
        [HttpGet("Ineligible")]
        public async Task<IEnumerable<MemberCouponDto>> GetIneligibleCoupon(int? MemberId)
        {
            if (MemberId == null)
            {
                return Enumerable.Empty<MemberCouponDto>();
            }
            var server = new MemberCouponService(_repo);
            return await server.GetIneligibleCoupon((int)MemberId);
        }
        [HttpGet("CanGet")]
        public async Task<IEnumerable<CouponCanGetDto>> GetCouponCanGet(int? MemberId)
        {
            if (MemberId == null)
            {
                return Enumerable.Empty<CouponCanGetDto>();
            }
            var server = new MemberCouponService(_repo);
            return await server.GetCouponCanGet((int)MemberId);
        }
		[HttpPut]
		public async Task<string> UseCouponAndCalculateDiscountPrice(int? memberId, int? memberCouponId)
		{
			if(memberId == null || memberCouponId==null)
			{
				return string.Empty;
			}
			var server = new MemberCouponService(_repo);
			if (!server.IsMemberExist((int)memberId)||!server.IsMemberCouponExist((int)memberCouponId))
			{
				return string.Empty;
			}
			if(!server.IsThisMemberHaveThisCoupon((int)memberId, (int)memberCouponId))
			{
				return string.Empty;
			}
			if (server.HasCouponBeenUsed((int)memberCouponId))
			{
                return string.Empty;
            }
			return await server.UseCouponAndCalculateDiscountPrice((int)memberId,(int)memberCouponId);
		}

		
	}
}

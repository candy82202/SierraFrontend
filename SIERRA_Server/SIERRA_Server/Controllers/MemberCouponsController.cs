using Microsoft.AspNetCore.Authorization;
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
		public async Task<IEnumerable<MemberCouponCanUseDto>> GetCouponMeetCriteria(int? MemberId)
		{
			if (MemberId == null)
			{
				return Enumerable.Empty<MemberCouponCanUseDto>();
			}
			var server = new MemberCouponService(_repo);
			return await server.GetCouponMeetCriteria((int)MemberId);
        }
        [HttpGet("Ineligible")]
        public async Task<IEnumerable<IneligibleMemberCouponDto>> GetIneligibleCoupon(int? MemberId)
        {
            if (MemberId == null)
            {
                return Enumerable.Empty<IneligibleMemberCouponDto>();
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
		public async Task<int> UseCouponAndCalculateDiscountPrice(int? memberId, int? memberCouponId)
		{
			if(memberId == null || memberCouponId==null)
			{
				return 0;
			}
			var server = new MemberCouponService(_repo);
			if (!server.IsMemberExist((int)memberId)||!server.IsMemberCouponExist((int)memberCouponId))
			{
				return 0;
			}
			if (!server.IsPromotionCouponAndReady((int)memberCouponId))
			{
				return 0;
			}
			if(!server.IsThisMemberHaveThisCoupon((int)memberId, (int)memberCouponId))
			{
				return 0;
			}
			if (server.HasCouponBeenUsed((int)memberCouponId))
			{
                return 0;
            }
			return await server.UseCouponAndCalculateDiscountPrice((int)memberId, (int)memberCouponId);
		}

		[HttpPost("DailyGame")]
		public async Task<AddCouponResult> PlayDailyGame(int? memberId)
		{
			if (memberId == null)
			{
				return AddCouponResult.Fail("查無此優惠券");
			}
			var server = new MemberCouponService(_repo);
			var result = await server.PlayDailyGame((int)memberId);
			return result;
		}
		[HttpGet("DailyGameRate")]
        [AllowAnonymous]
        public async Task<IEnumerable<DailyGameRateDto>> GetDailyGameRate()
		{
			var server = new MemberCouponService(_repo);
			var result = await server.GetDailyGameRate();
			return result;
        }
		[HttpPost("WeeklyGame")]
		public async Task<WeeklyGameResult?> PlayWeeklyGame(int[] ansAry,int? memberId)
		{
			
			if(ansAry.Length != 5 || memberId==null)
			{
				return null;
			}
			Thread.Sleep(1200);
			var server = new MemberCouponService(_repo);
			var result = await server.PlayWeeklyGame(ansAry,(int)memberId);
			return result;
		}
		[HttpPut("CancelCoupon")]
		public async Task<bool> CancelUsingCoupon(int? memberId)
		{
			if (memberId == null)
			{
				return false;
			}
            var server = new MemberCouponService(_repo);
			var result = await server.CancelUsingCoupon((int)memberId);
			return result;
        }
		[HttpGet("GetUsingCoupon")]
		public async Task<object?> GetUsingCoupon(int? memberId)
		{
			if(memberId == null)
			{
				return null;
			}
			var server = new MemberCouponService(_repo);
			var result = await server.GetUsingCoupon((int)memberId);
			return result;

        }
		[HttpGet("DidMemberPlayedGame")]
		public async Task<object?> DidMemberPlayedGame(int? memberId)
		{
			if (memberId == null)
			{
				return null;
			}
			var server = new MemberCouponService(_repo);
			var result = await server.DidMemberPlayedGame((int)memberId);
			return result;

        }

    }
}

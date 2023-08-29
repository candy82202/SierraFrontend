using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Services;

namespace SIERRA_Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class HangFireController : ControllerBase
	{
		private IMemberCouponRepository _repo;
        public HangFireController(IMemberCouponRepository repo,AppDbContext db)
        {
			_repo = repo;

        }
		[HttpPut]
		public void LetMembersCanPlayDailyGame()
		{
            var server = new MemberCouponService(_repo);
            RecurringJob.AddOrUpdate(() => server.LetMembersCanPlayDailyGame(), "0 16 * * *");
		}
		[HttpPut("Weekly")]
		public void LetMembersCanPlayWeeklyGame()
		{
            var server = new MemberCouponService(_repo);
			RecurringJob.AddOrUpdate(() => server.LetMembersCanPlayWeeklyGame(), Cron.Weekly(DayOfWeek.Monday, 16, 0));
        }

		[HttpPost]
		public void GiveMemberCouponWhoBirthInThisMonth()
		{
			var server = new MemberCouponService(_repo);
			RecurringJob.AddOrUpdate(() => server.GiveMemberCouponWhoBirthInThisMonth(), Cron.Monthly(1, 16, 0));
		}

	}
}

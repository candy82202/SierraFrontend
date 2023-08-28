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
		private AppDbContext _db;
        public HangFireController(IMemberCouponRepository repo,AppDbContext db)
        {
			_repo = repo;
			_db = db;

        }
		[HttpPut]
		public void LetMembersCanPlayDaailyGame()
		{
            var server = new MemberCouponService(_repo);
            RecurringJob.AddOrUpdate(() => server.LetMembersCanPlayDaailyGame(), Cron.Daily);
		}
	}
}

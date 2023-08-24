using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Repository.EFRepository;
using SIERRA_Server.Models.Services;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private PromotionEFRepository _repo;
        public PromotionsController(PromotionEFRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<PromotionDto>> GetPromotionsNow()
        {
            var server = new PromotionService(_repo);
            var promotions = await server.GetPromotionsNow();
            return promotions;
        }
        [HttpPost]
        public async Task<AddCouponResult> GetCouponByPromotion(int? memberId ,int? couponId)
        {
            if (memberId == null || couponId == null)
            {
                return AddCouponResult.Fail("查無此優惠券");
            }
            var server = new PromotionService(_repo);
            var result = await server.GetCouponByPromotion((int)memberId, (int)couponId);
            return result;
        }
    }
}

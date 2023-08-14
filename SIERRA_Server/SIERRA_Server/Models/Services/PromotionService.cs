using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.Exts;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Repository.EFRepository;

namespace SIERRA_Server.Models.Services
{
    public class PromotionService
    {
        private PromotionEFRepository _repo;

        public PromotionService(PromotionEFRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PromotionDto>> GetPromotionsNow()
        {
            var promotions= await _repo.GetPromotionsNow();
            var result = promotions.Select(p => p.ToPromotionDto());
            return result;
        }

        public async Task<AddCouponResult> GetCouponByPromotion(int memberId, int couponId)
        {
            //檢查484 promotion coupon
            if (!await _repo.IsPromotionCoupon(couponId))
            {
                return AddCouponResult.Fail("查無此優惠券");
            }
            //檢查有沒有領取過
            if (await _repo.HasGottenCoupon(memberId,couponId))
            {
                return AddCouponResult.Fail("您已經領取過此優惠券囉");
            }
            //領取
            var result=await _repo.GetPromotionCoupon(memberId, couponId);
            return result;

        }
    }
}

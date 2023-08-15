using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetPromotionsNow();
        Task<bool> HasGottenCoupon(int memberId, int couponId);
        Task<bool> IsPromotionCoupon(int couponId);
        Task<AddCouponResult> GetPromotionCoupon(int memberId, int couponId);
    }
}

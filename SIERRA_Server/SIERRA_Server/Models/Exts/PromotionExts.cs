using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Exts
{
    public static class PromotionExts
    {
        public static PromotionDto ToPromotionDto(this Promotion entity)
        {
            return new PromotionDto()
            {
                PromotionId = entity.PromotionId,
                CouponId = entity.CouponId,
                PromotionName = entity.PromotionName,
                PromotionImage = entity.PromotionImage,
                Description = entity.Description,
                LaunchAt = entity.LaunchAt,
                StartAt = entity.StartAt,
                EndAt = entity.EndAt,
            };
        }
    }
}

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
    }
}

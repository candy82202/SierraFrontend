using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class PromotionEFRepository : IPromotionRepository
    {
        private readonly AppDbContext _db;
        public PromotionEFRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Promotion>> GetPromotionsNow()
        {
            var promotions =await _db.Promotions.Where(p=>p.LaunchAt<DateTime.Now&&p.EndAt>DateTime.Now)
                                                .ToListAsync();
            return promotions;
        }
    }
}

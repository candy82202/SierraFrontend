using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class DessertCategoryEFRepository : IDessertCategoryRepository
    {
        private readonly AppDbContext _context;

        public DessertCategoryEFRepository(AppDbContext db)
        {
            _context = db;
        }

        public async Task<List<Dessert>> GetDessertsByCategoryId(int categoryId)
        {
            return await _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Where(d => d.Status && d.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}

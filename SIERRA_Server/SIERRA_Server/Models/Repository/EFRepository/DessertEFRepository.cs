using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class DessertEFRepository : IDessertRepository
    {
        private readonly AppDbContext _context;

        public DessertEFRepository(AppDbContext db)
        {
            _context = db;
        }
        public async Task<List<DessertListDTO>> GetHotProductsAsync()
        {
            var dvm = new List<DessertListDTO>();

            var hotProductIds = await _context.DessertOrderDetails
                .GroupBy(d => d.DessertId)
                .OrderByDescending(g => g.Sum(d => d.Quantity))
                .Take(3)
                .Select(g => g.Key)
                .ToListAsync();

            var hotProductsQuery = _context.Desserts
                .Where(d => hotProductIds.Contains(d.DessertId))
                .Include(d => d.Specifications)
                .Include(d => d.DessertImages);

            var hotProducts = await hotProductsQuery.ToListAsync();

            var hotProductsDTO = hotProducts
                .OrderBy(d => hotProductIds.IndexOf(d.DessertId)) // 在内存中进行排序
                .Select(d => d.ToDListDto())
               
                .ToList();

            dvm.AddRange(hotProductsDTO);
            return dvm;
        }
    }
}

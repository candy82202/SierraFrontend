
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Services;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IDessertRepository _repo;
        public DessertsController(AppDbContext context, IConfiguration config,IDessertRepository repo)
        {
            _context = context;
            _configuration = config;
            _repo = repo;
        }
        // GET: api/Desserts/moldCake
        [HttpGet("moldCake")]
        public IActionResult GetMoldCake()
        {
            var dvm = new List<DessertsIndexDTO>();

            var desserts = _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Where(d => d.Status && d.CategoryId == 1)
                .ToList();

            foreach (var dessert in desserts)
            {
                // Fetching UnitPrice from Specifications
                var specification = dessert.Specifications.FirstOrDefault();
                int unitPrice = specification?.UnitPrice ?? 0;

                DessertsIndexDTO item = new DessertsIndexDTO
                {
                    DessertName = dessert.DessertName,
                    UnitPrice = unitPrice,
                    DessertImageName = dessert.DessertImages.FirstOrDefault()?.DessertImageName
                };
                dvm.Add(item);
            }

            return Ok(dvm); // 將結果轉為 JSON 格式並回傳
        }
        // GET: api/Desserts/roomTemperature
        [HttpGet("roomTemperature")]
        public IActionResult GetRoomTemperature()
        {
            var dvm = new List<DessertsIndexDTO>();

            var desserts = _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Where(d => d.Status && d.CategoryId == 2)
                .ToList();

            foreach (var dessert in desserts)
            {
                // Fetching UnitPrice from Specifications
                var specification = dessert.Specifications.FirstOrDefault();
                int unitPrice = specification?.UnitPrice ?? 0;

                DessertsIndexDTO item = new DessertsIndexDTO
                {
                    DessertName = dessert.DessertName,
                    UnitPrice = unitPrice,
                    DessertImageName = dessert.DessertImages.FirstOrDefault()?.DessertImageName
                };
                dvm.Add(item);
            }

            return Ok(dvm); // 將結果轉為 JSON 格式並回傳
        }
        // GET: api/Desserts/snack
        [HttpGet("snack")]
        public IActionResult GetSnack()
        {
            var dvm = new List<DessertsIndexDTO>();

            var desserts = _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Where(d => d.Status && d.CategoryId == 3)
                .ToList();

            foreach (var dessert in desserts)
            {
                // Fetching UnitPrice from Specifications
                var specification = dessert.Specifications.FirstOrDefault();
                int unitPrice = specification?.UnitPrice ?? 0;

                DessertsIndexDTO item = new DessertsIndexDTO
                {
                    DessertName = dessert.DessertName,
                    UnitPrice = unitPrice,
                    DessertImageName = dessert.DessertImages.FirstOrDefault()?.DessertImageName
                };
                dvm.Add(item);
            }

            return Ok(dvm); // 將結果轉為 JSON 格式並回傳
        }
        // GET: api/Desserts/longCake
        [HttpGet("longCake")]
        public IActionResult GetLongCake()
        {
            var dvm = new List<DessertsIndexDTO>();

            var desserts = _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Where(d => d.Status && d.CategoryId == 4)
                .ToList();

            foreach (var dessert in desserts)
            {
                // Fetching UnitPrice from Specifications
                var specification = dessert.Specifications.FirstOrDefault();
                int unitPrice = specification?.UnitPrice ?? 0;

                DessertsIndexDTO item = new DessertsIndexDTO
                {
                    DessertName = dessert.DessertName,
                    UnitPrice = unitPrice,
                    DessertImageName = dessert.DessertImages.FirstOrDefault()?.DessertImageName
                };
                dvm.Add(item);
            }

            return Ok(dvm); // 將結果轉為 JSON 格式並回傳
        }
        // GET: api/Desserts/presents
        [HttpGet("presents")]
        public IActionResult GetPresents()
        {
            var dvm = new List<DessertsIndexDTO>();

            var desserts = _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Where(d => d.Status && d.CategoryId == 5)
                .ToList();

            foreach (var dessert in desserts)
            {
                // Fetching UnitPrice from Specifications
                var specification = dessert.Specifications.FirstOrDefault();
                int unitPrice = specification?.UnitPrice ?? 0;

                DessertsIndexDTO item = new DessertsIndexDTO
                {
                    DessertName = dessert.DessertName,
                    UnitPrice = unitPrice,
                    DessertImageName = dessert.DessertImages.FirstOrDefault()?.DessertImageName
                };
                dvm.Add(item);
            }

            return Ok(dvm); // 將結果轉為 JSON 格式並回傳
        }
        [HttpGet("TopSalesDesserts")]
        public async Task<IActionResult> TopSaleDesserts()
        {
            //List<DessertListDTO> hotProducts = await GetHotProductsAsync();
            var service = new DessertService(_repo);
            var hotProducts = await service.GetHotProductsAsync();

            return Ok(hotProducts);
        }
        // GET: api/Desserts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dessert>> GetDessert(int id=3)
        {
            var dvm = new List<DessertDTO>();

            var desserts = _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Where(d => d.DessertId == id)
                .ToList();

            foreach (var dessert in desserts)
            {
                // Fetching UnitPrice from Specifications
                var specification = dessert.Specifications.FirstOrDefault();
                int unitPrice = specification?.UnitPrice ?? 0;

                DessertDTO item = new DessertDTO
                {
                    DessertId = dessert.DessertId,
                    DessertName = dessert.DessertName,
                    UnitPrice = unitPrice,
                    Description = dessert.Description,
                    DessertImageName = dessert.DessertImages.FirstOrDefault()?.DessertImageName
                };
                dvm.Add(item);
            }

            return Ok(dvm); // 將結果轉為 JSON 格式並回傳
        }
        private async Task<List<DessertListDTO>> GetHotProductsAsync()
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
                .OrderBy(d => hotProductIds.IndexOf(d.DessertId)) // 在内存中進行排序
                .Select(d => new DessertListDTO
                {
                    Dessert = d,
                    DessertId = d.DessertId,
                    DessertName = d.DessertName,
                    DessertImageName = d.DessertImages.FirstOrDefault()?.DessertImageName,
                    UnitPrice = d.Specifications.FirstOrDefault()?.UnitPrice ?? 0,
                })
                .ToList();

            dvm.AddRange(hotProductsDTO);
            return dvm;
        }
    }
}

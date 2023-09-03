
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Services;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DessertsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IDessertRepository _repo;
        private readonly IDessertDiscountRepository _discountrepo;
        public DessertsController(AppDbContext context, IConfiguration config, IDessertRepository repo, IDessertDiscountRepository discountrepo)
        {
            _context = context;
            _configuration = config;
            _repo = repo;
            _discountrepo = discountrepo;
        }
        private decimal CalculateDiscountedPrice(int unitPrice, decimal dessertDiscountPrice)
        {
            return dessertDiscountPrice != 0
                ? Math.Round(unitPrice * (dessertDiscountPrice / 100), 0, MidpointRounding.AwayFromZero)
                : unitPrice;
        }

        [HttpGet]
        public async Task<ActionResult<List<DessertPageDTO>>> GetProducts(string? keyword, int page = 1, int pageSize = 4)
        {
            if (_context.Desserts == null)
            {
                return NotFound();
            }

            var productsQuery = _context.Desserts
     .Include(d => d.Specifications)
     .Include(d => d.DessertImages)
     .Include(d=> d.Discounts)
     .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                productsQuery = productsQuery.Where(p => p.DessertName.Contains(keyword));
            }

            int totalCount = await productsQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedProducts = await productsQuery
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            var dessertDTOs = pagedProducts.Select(d =>
            {
                decimal dessertDiscountPrice = d.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
                     ? d.Discounts.First().DiscountPrice
                     : 0;
                var unitPrice = d.Specifications?.FirstOrDefault()?.UnitPrice ?? 0;
                var specificationId = d.Specifications?.FirstOrDefault()?.SpecificationId ?? 0;
                var imageName = d.DessertImages?.FirstOrDefault()?.DessertImageName;


                return new DessertPageDTO
                {
                    DessertId = d.DessertId,
                    DessertName = d.DessertName,
                    SpecificationId = specificationId,
                    UnitPrice = unitPrice,
                    DessertDiscountPrice = dessertDiscountPrice,
                    ImageName = imageName,
                    TotalPages = totalPages
                };
            }).ToList();

            return dessertDTOs;

            ////要做搜尋功能，這裡先讀出所有資料。
            //var products = _context.Desserts.AsQueryable();

            ////要做產品名稱的搜尋，如果keyword存在的話
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    products = products.Where(p => p.DessertName.Contains(keyword));
            //}

            ////分頁
            //int totalCount = products.Count();//這裡有總共幾筆 10
            ////int pageSize = 3;//每頁顯示三筆資料，這裡可以從前端傳進來，自由選擇。(這裡暫時寫死)
            //int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize); //轉換型態計算出總共有幾頁 4
            //                                                                   // int page = 0;//一開始是0

            //// 執行一次資料庫查詢並設定結果到 DTO
            //var pagedProducts = await products.Include(p => p.Specifications)
            //                                  .Include(p => p.DessertImages)
            //                                  .Skip(pageSize * (page - 1))
            //                                  .Take(pageSize)
            //                                  .ToListAsync();

            //DessertPageDTO dessertDTO = new DessertPageDTO();
            //dessertDTO.Dessert = pagedProducts; // 設定查詢結果
            //dessertDTO.UnitPrice = pagedProducts.FirstOrDefault()?.Specifications.FirstOrDefault()?.UnitPrice ?? 0;

            //dessertDTO.ImageName = pagedProducts.FirstOrDefault()?.DessertImages.FirstOrDefault()?.DessertImageName;
            //dessertDTO.TotalPages = totalPages;

            //return dessertDTO; // 返回 DTO 物件
        }


        // GET: api/Desserts/moldCake
        [HttpGet("moldCake")]
        public async Task<IActionResult> GetMoldCake()
        {
            var service = new DessertService(_repo);
            var moldCake = await service.GetMoldCake();
            return Ok(moldCake);
        }
        // GET: api/Desserts/roomTemperature
        [HttpGet("roomTemperature")]
        public async Task<IActionResult> GetRoomTemperature()
        {
            var service = new DessertService(_repo);
            var roomTemperature = await service.GetRoomTemperature();
            return Ok(roomTemperature);
        }
        // GET: api/Desserts/snack
        [HttpGet("snack")]
        public async Task<IActionResult> GetSnack()
        {
            var service = new DessertService(_repo);
            var snack = await service.GetSnack();
            return Ok(snack);
        }

        // GET: api/Desserts/longCake
        [HttpGet("longCake")]
        public async Task<IActionResult> GetLongCake()
        {
            var service = new DessertService(_repo);
            var longCake = await service.GetLongCake();
            return Ok(longCake);
        }

        // GET: api/Desserts/presents
        [HttpGet("presents")]
        public async Task<IActionResult> GetPresents()
        {
            var service = new DessertService(_repo);
            var presents = await service.GetPresents();
            return Ok(presents);
        }

        [HttpGet("TopSalesDesserts")]
        public async Task<IActionResult> TopSaleDesserts()
        {
            var service = new DessertService(_repo);
            var hotProducts = await service.GetHotProductsAsync();

            return Ok(hotProducts);
        }
        // GET: api/Desserts/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Dessert>> GetDessert(int id)
        //{
        //    var dvm = new List<DessertDTO>();

        //    var desserts = _context.Desserts
        //        .Include(d => d.Category)
        //        .Include(d => d.DessertImages)
        //        .Include(d => d.Specifications)
        //        .Include(d => d.Discounts)
        //        .Where(d => d.DessertId == id)
        //        .ToList();

        //    foreach (var dessert in desserts)
        //    {
        //        decimal dessertDiscountPrice = dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
        //     ? dessert.Discounts.First().DiscountPrice
        //     : 0;

        //        DessertDTO item = new DessertDTO(dessertDiscountPrice, dessert.Specifications.First().UnitPrice)
        //        {
        //            DessertId = dessert.DessertId,
        //            DessertName = dessert.DessertName,
        //            CategoryName = dessert.Category.CategoryName,
        //            UnitPrice = dessert.Specifications.First().UnitPrice,
        //            Description = dessert.Description,
        //            DessertImages = dessert.DessertImages?.ToList(),
        //            Specifications = dessert.Specifications.Select(spec =>
        //                new SpecificationDTO(dessertDiscountPrice, dessert.Specifications.UnitPrice)
        //                {
        //                    SpecificationId = spec.SpecificationId,
        //                    UnitPrice = spec.UnitPrice,
        //                    Size = spec.Size,
        //                    Flavor = spec.Flavor,

        //                    // Include other properties here
        //                }
        //            ).ToList()
        //        };
        //        dvm.Add(item);
        //    }

        //    return Ok(dvm); // 將結果轉為 JSON 格式並回傳
        //}
        [HttpGet("{id}")]
        public async Task<ActionResult<List<DessertDTO>>> GetDessert(int id)
        {
            var dvm = new List<DessertDTO>();
            var desserts = await _context.Desserts
                .Include(d => d.Category)
                .Include(d => d.DessertImages)
                .Include(d => d.Specifications)
                .Include(d => d.Discounts)
                .Where(d => d.DessertId == id)
                .ToListAsync();

            foreach (var dessert in desserts)
            {
                decimal dessertDiscountPrice = dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
                    ? dessert.Discounts.First().DiscountPrice
                    : 0;

                var specifications = dessert.Specifications.Select(spec =>
                {
                    // Calculate specification discount price based on dessert discount
                    var specificationDiscountPrice = spec.UnitPrice * (dessertDiscountPrice / 100);

                    var specification = new SpecificationDTO(specificationDiscountPrice, spec.UnitPrice)
                    {
                        SpecificationId = spec.SpecificationId,
                        UnitPrice = spec.UnitPrice,
                        Size = spec.Size,
                        Flavor = spec.Flavor

                    };

                    return specification;
                }).ToList();

                var item = new DessertDTO(dessertDiscountPrice, specifications.First().UnitPrice)
                {
                    DessertId = dessert.DessertId,
                    DessertName = dessert.DessertName,
                    CategoryName = dessert.Category.CategoryName,
                    UnitPrice = specifications.First().UnitPrice,
                    Description = dessert.Description,
                    DessertImages = dessert.DessertImages?.ToList(),
                    Specifications = specifications
                };

                dvm.Add(item);
            }

            return Ok(dvm);
        }



        /// <summary>
        /// DiscountGroup Method
        /// </summary>
        /// <returns></returns>
        [HttpGet("ChocoDiscountGroups")]
        public async Task<ActionResult<List<DessertDiscountDTO>>> GetChocoDiscountGroups()
        {
            var service = new DessertService(_discountrepo);
            var chocoDiscount = await service.GetChocoDiscountGroups();

            return Ok(chocoDiscount);
        }
        [HttpGet("StrawberryDiscountGroups")]
        public async Task<ActionResult<List<DessertDiscountDTO>>> GetStrawberryDiscountGroups()
        {
            var service = new DessertService(_discountrepo);
            var strawberryDiscount = await service.GetStrawberryDiscountGroups();
            return Ok(strawberryDiscount);
        }
        [HttpGet("MochaDiscountGroups")]
        public async Task<ActionResult<List<DessertDiscountDTO>>> GetMochaDiscountGroups()
        {
            var service = new DessertService(_discountrepo);
            var mochaDiscount = await service.GetMochaDiscountGroups();
            return Ok(mochaDiscount);
        }
        [HttpGet("TaroDiscountGroups")]
        public async Task<ActionResult<List<DessertDiscountDTO>>> GetTaroDiscountGroups()
        {
            var service = new DessertService(_discountrepo);
            var taroDiscount = await service.GetTaroDiscountGroups();
            return Ok(taroDiscount);
        }
        [HttpGet("AlcoholDiscountGroups")]
        public async Task<ActionResult<List<DessertDiscountDTO>>> GetAlcoholDiscountGroups()
        {
            var service = new DessertService(_discountrepo);
            var alcoholDiscount = await service.GetAlcoholDiscountGroups();
            return Ok(alcoholDiscount);
        }
        [HttpGet("SuggestDiscountGroups")]
        public async Task<ActionResult<List<DessertDiscountDTO>>> GetSuggestDiscountGroups(int dessertId)
        {
            var service = new DessertService(_discountrepo);
            var suggestDiscount = await service.GetSuggestDiscountGroups(dessertId);
            return Ok(suggestDiscount);
        }
    }
}

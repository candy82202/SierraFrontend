
using Dapper;
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
    public class DessertsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IDessertRepository _repo;
        private readonly IDessertDiscountRepository _discountrepo;
        public DessertsController(AppDbContext context, IConfiguration config,IDessertRepository repo, IDessertDiscountRepository discountrepo)
        {
            _context = context;
            _configuration = config;
            _repo = repo;
            _discountrepo = discountrepo;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Dessert>> GetDessert(int id)
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
                //var specification = dessert.Specifications.FirstOrDefault();
                //int unitPrice = specification?.UnitPrice ?? 0;
                List<SpecificationDTO> specifications = dessert.Specifications.Select(spec =>
           new SpecificationDTO
           {
               SpecificationId=spec.SpecificationId,
               UnitPrice = spec.UnitPrice,
               Size = spec.Size,
               Flavor = spec.Flavor,
               // Include other properties here
           }
       ).ToList();
                DessertDTO item = new DessertDTO
                {
                    DessertId = dessert.DessertId,
                    DessertName = dessert.DessertName,
                    //UnitPrice = specifications.Select(spec => spec.UnitPrice).ToList(),
                    Description = dessert.Description,
                    DessertImageName = dessert.DessertImages.FirstOrDefault()?.DessertImageName,
                    Specifications = specifications,
                 
                };
                dvm.Add(item);
            }

            return Ok(dvm); // 將結果轉為 JSON 格式並回傳
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

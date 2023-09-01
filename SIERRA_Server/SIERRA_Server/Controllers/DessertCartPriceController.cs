using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.DTOs.Carts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Services;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertCartPriceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IDessertCartRepository _repo;

        public DessertCartPriceController(AppDbContext context, IDessertCartRepository repo)
        {
            _repo = repo;

        }

        [HttpGet("GetCart")]
        public async Task<CartDTO> GetOrCreateCart(string username)
        {
            var cartDto = await _repo.GetOrCreateCart(username);
            return cartDto;
        }
 


        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] AddOrUpdateCartItemRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }
            var service = new DessertCartService(_repo);
            await service.AddOrUpdateCartItem(request.Username, request.DessertId, request.SpecificationId, request.Quantity);

            return Ok();
        }
        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCartItemQty([FromBody] UpdateCartItemRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }
            var service = new DessertCartService(_repo);
            await service.UpdateCartItemQty(request.id,  request.Quantity);

            return Ok();
        }
        [HttpGet("GetPrice")]
        public async Task<int> GetCartTotalPrice(string username)
        {
            var cartPrice = await _repo.GetCartTotalPrice(username);
            return cartPrice;        

        }
    }
}

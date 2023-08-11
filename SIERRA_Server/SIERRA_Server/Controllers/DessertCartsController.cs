using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Carts;
using SIERRA_Server.Models.DTOs.Orders;
using SIERRA_Server.Models.EFModels;


namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertCartsController : ControllerBase
    {
        private readonly AppDbContext _context;
      
        public DessertCartsController(AppDbContext context)
        {
            _context = context;
       
        }
        // GET: api/DessertCarts
        [HttpGet]
        public async Task<IEnumerable<DessertCartItemsDto>> GetDessertCarts(string? username)
        {
            if (_context.DessertCarts == null || username == null)
            {
                return Enumerable.Empty<DessertCartItemsDto>();
            }
            var cart = await _context.DessertCarts.Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Dessert).ThenInclude(d => d.DessertImages).FirstOrDefaultAsync(dc => dc.Username == username);
            if (cart == null)
            {
                return Enumerable.Empty<DessertCartItemsDto>();
            }
            var cartItems = cart.DessertCartItems.Select(dci => new DessertCartItemsDto()
            {
                DessertCartItemId = dci.Id,
                DessertName = dci.Dessert.DessertName,
                DessertImage = dci.Dessert.DessertImages?.OrderBy(di => di.ImageId).Select(di => di.DessertImageName).First(),
                Price = _context.Specifications.Find(dci.SpecificationId).UnitPrice,
                Count = dci.Quantity
            });
            return cartItems;
        }

        // GET: api/DessertCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DessertCart>> GetDessertCart(int id)
        {
            if (_context.DessertCarts == null)
            {
                return NotFound();
            }
            var dessertCart = await _context.DessertCarts.FindAsync(id);

            if (dessertCart == null)
            {
                return NotFound();
            }

            return dessertCart;
        }


        // PUT: api/DessertCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDessertCart(int id, DessertCart dessertCart)
        {
            if (id != dessertCart.Id)
            {
                return BadRequest();
            }

            _context.Entry(dessertCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DessertCartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DessertCarts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DessertCart>> PostDessertCart(DessertCart dessertCart)
        {
            if (_context.DessertCarts == null)
            {
                return Problem("Entity set 'AppDbContext.DessertCarts'  is null.");
            }
            _context.DessertCarts.Add(dessertCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDessertCart", new { id = dessertCart.Id }, dessertCart);
        }

        // DELETE: api/DessertCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDessertCart(int id)
        {
            if (_context.DessertCarts == null)
            {
                return NotFound();
            }
            var dessertCart = await _context.DessertCartItems.FindAsync(id);
          

            if (dessertCart == null)
            {
                return NotFound();
            }

            _context.DessertCartItems.Remove(dessertCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        //[HttpGet("GetCartItems")]
        //public async Task<CartDTO> GetDessertCartItems(string? username)
        //{

        //    var cart = await _context.DessertCarts
        //        .Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Dessert).ThenInclude(d => d.DessertImages)
        //         .Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Specification)
        //        .FirstOrDefaultAsync(dc => dc.Username == username);
        //    if (cart == null)
        //    {
        //        cart = new DessertCart
        //        {
        //            Username = username
        //        };
        //        _context.DessertCarts.Add(cart);
        //        await _context.SaveChangesAsync();
        //    }
        //    var cartItems = cart.DessertCartItems.Select(dci => new DessertCartItemsDto()
        //    {
        //        DessertCartItemId = dci.Id,
        //        DessertName = dci.Dessert.DessertName,
        //        DessertImage = dci.Dessert.DessertImages?.OrderBy(di => di.ImageId).Select(di => di.DessertImageName).First(),
        //        Price = _context.Specifications.Find(dci.SpecificationId).UnitPrice,
        //        Count = dci.Quantity
        //    });
        //    return cartItems;
        //}
        [HttpGet("GetCart")]
        public async Task<CartDTO> GetOrCreateCart(string username)
        {
            var cart = await _context.DessertCarts
                .Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Dessert).ThenInclude(d=>d.Discounts)
				.Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Dessert).ThenInclude(d => d.DessertImages)
				.Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Specification)
                .FirstOrDefaultAsync(dc => dc.Username == username);

            if (cart == null)
            {
                cart = new DessertCart
                {
                    Username = username,
                    MemberCouponId = null
                };
                _context.DessertCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartDTO = new CartDTO
            {
                Id = cart.Id,
                Username = cart.Username,
                DessertCartItems = cart.DessertCartItems.Select(dci => new CartItemDTO
                {
                    Id = dci.Id,
                    SpecificationId = dci.SpecificationId,
                    DessertCartId = dci.DessertCartId,
                    DessertId = dci.DessertId,
                    Quantity = dci.Quantity,
                    DessertImageName = dci.Dessert.DessertImages.FirstOrDefault()?.DessertImageName,
                    DessertName = dci.Dessert.DessertName,
                    UnitPrice = (int)(dci.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)? Math.Round((decimal)dci.Specification.UnitPrice * ((decimal)dci.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : dci.Specification.UnitPrice)
				}).ToList()
            };

            return cartDTO;
        }
        [HttpPost("AddToCart")]
        public async Task AddOrUpdateCartItem(string username, int dessertId, int specificationId, int quantity)
        {
            var cart = await GetOrCreateCart(username);
            var existingItem = cart.DessertCartItems.FirstOrDefault(item => item.DessertId == dessertId && item.SpecificationId == specificationId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _context.DessertCartItems.Find(existingItem.Id).Quantity=existingItem.Quantity;

            }
            else
            {
                var newItem = new DessertCartItem
                {
                    DessertCartId = cart.Id,
                    DessertId = dessertId,
                    SpecificationId = specificationId,
                    Quantity = quantity
                };
                _context.DessertCartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
        }
        [HttpPut("UpdateCart")]
        public async Task UpdateCartItemQty( int id, int quantity)
        {
            
            var cartItem = _context.DessertCartItems.FirstOrDefault(item => item.Id == id);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                if (cartItem.Quantity < 1)
                {
                    // 如果数量小于1，将项目从购物车中删除
                    _context.DessertCartItems.Remove(cartItem);
                }
                else
                {
                    _context.DessertCartItems.Update(cartItem);
                }

                await _context.SaveChangesAsync();
            }
        }
        [HttpGet("GetPrice")]
        public async Task<int> GetCartTotalPrice(string username)
        {
            var cart = await GetOrCreateCart(username);
            return cart.DessertCartItems.Sum(item => item.SubTotal);
        }
        [HttpGet("Checkout")]
        public async Task<bool> CanCheckout(string username)
        {
            var cart = await GetOrCreateCart(username);
            return cart.DessertCartItems.Any();
        }
   
    private bool DessertCartExists(int id)
        {
            return (_context.DessertCarts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

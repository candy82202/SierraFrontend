using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private bool DessertCartExists(int id)
        {
            return (_context.DessertCarts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

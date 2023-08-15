using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertCartItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DessertCartItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DessertCartItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DessertCartItem>>> GetDessertCartItems()
        {
          if (_context.DessertCartItems == null)
          {
              return NotFound();
          }
            return await _context.DessertCartItems.ToListAsync();
        }

        // GET: api/DessertCartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DessertCartItem>> GetDessertCartItem(int id)
        {
          if (_context.DessertCartItems == null)
          {
              return NotFound();
          }
            var dessertCartItem = await _context.DessertCartItems.FindAsync(id);

            if (dessertCartItem == null)
            {
                return NotFound();
            }

            return dessertCartItem;
        }

        // PUT: api/DessertCartItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDessertCartItem(int id, DessertCartItem dessertCartItem)
        {
            if (id != dessertCartItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(dessertCartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DessertCartItemExists(id))
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

        // POST: api/DessertCartItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DessertCartItem>> PostDessertCartItem(DessertCartItem dessertCartItem)
        {
          if (_context.DessertCartItems == null)
          {
              return Problem("Entity set 'AppDbContext.DessertCartItems'  is null.");
          }
            _context.DessertCartItems.Add(dessertCartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDessertCartItem", new { id = dessertCartItem.Id }, dessertCartItem);
        }

        // DELETE: api/DessertCartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDessertCartItem(int id)
        {
            if (_context.DessertCartItems == null)
            {
                return NotFound();
            }
            var dessertCartItem = await _context.DessertCartItems.FindAsync(id);
            var cart = await _context.DessertCarts.FindAsync(dessertCartItem.DessertCartId);
            if (dessertCartItem == null)
            {
                return NotFound();
            }

            cart.MemberCouponId = null;
            _context.DessertCartItems.Remove(dessertCartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DessertCartItemExists(int id)
        {
            return (_context.DessertCartItems?.Any(e => e.Id == id)).GetValueOrDefault(); 
        }
    }
}

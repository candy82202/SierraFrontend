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
    public class DessertOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DessertOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DessertOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DessertOrder>>> GetDessertOrders()
        {
          if (_context.DessertOrders == null)
          {
              return NotFound();
          }
            return await _context.DessertOrders.ToListAsync();
        }

        // GET: api/DessertOrders/5
        [HttpGet]
        public async Task<IEnumerable<MemberItemDTO>> GetCustomerData(string? memberName)
        {
          if (_context.DessertOrders == null || memberName == null)
          {
                return Enumerable.Empty<MemberItemDTO>();
            }
            var customerData = await _context.DessertOrders.Include(c=>c.MemberId).Include(ci=>ci.)

            if (dessertOrder == null)
            {
                return NotFound();
            }

            return dessertOrder;
        }

        // PUT: api/DessertOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDessertOrder(int id, DessertOrder dessertOrder)
        {
            if (id != dessertOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(dessertOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DessertOrderExists(id))
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

        // POST: api/DessertOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DessertOrder>> PostDessertOrder(DessertOrder dessertOrder)
        {
          if (_context.DessertOrders == null)
          {
              return Problem("Entity set 'AppDbContext.DessertOrders'  is null.");
          }
            _context.DessertOrders.Add(dessertOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDessertOrder", new { id = dessertOrder.Id }, dessertOrder);
        }

        // DELETE: api/DessertOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDessertOrder(int id)
        {
            if (_context.DessertOrders == null)
            {
                return NotFound();
            }
            var dessertOrder = await _context.DessertOrders.FindAsync(id);
            if (dessertOrder == null)
            {
                return NotFound();
            }

            _context.DessertOrders.Remove(dessertOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DessertOrderExists(int id)
        {
            return (_context.DessertOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

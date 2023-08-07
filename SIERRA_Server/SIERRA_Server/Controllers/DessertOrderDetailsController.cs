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
    public class DessertOrderDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DessertOrderDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DessertOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DessertOrderDetail>>> GetDessertOrderDetails()
        {
          if (_context.DessertOrderDetails == null)
          {
              return NotFound();
          }
            return await _context.DessertOrderDetails.ToListAsync();
        }

        // GET: api/DessertOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DessertOrderDetail>> GetDessertOrderDetail(int id)
        {
          if (_context.DessertOrderDetails == null)
          {
              return NotFound();
          }
            var dessertOrderDetail = await _context.DessertOrderDetails.FindAsync(id);

            if (dessertOrderDetail == null)
            {
                return NotFound();
            }

            return dessertOrderDetail;
        }

        // PUT: api/DessertOrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDessertOrderDetail(int id, DessertOrderDetail dessertOrderDetail)
        {
            if (id != dessertOrderDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(dessertOrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DessertOrderDetailExists(id))
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

        // POST: api/DessertOrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DessertOrderDetail>> PostDessertOrderDetail(DessertOrderDetail dessertOrderDetail)
        {
          if (_context.DessertOrderDetails == null)
          {
              return Problem("Entity set 'AppDbContext.DessertOrderDetails'  is null.");
          }
            _context.DessertOrderDetails.Add(dessertOrderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDessertOrderDetail", new { id = dessertOrderDetail.Id }, dessertOrderDetail);
        }

        // DELETE: api/DessertOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDessertOrderDetail(int id)
        {
            if (_context.DessertOrderDetails == null)
            {
                return NotFound();
            }
            var dessertOrderDetail = await _context.DessertOrderDetails.FindAsync(id);
            if (dessertOrderDetail == null)
            {
                return NotFound();
            }

            _context.DessertOrderDetails.Remove(dessertOrderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DessertOrderDetailExists(int id)
        {
            return (_context.DessertOrderDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

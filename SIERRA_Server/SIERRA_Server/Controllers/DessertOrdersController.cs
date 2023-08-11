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

        //GET: api/DessertOrders/5
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberItemDTO>> GetCustomerData(string? username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var userData=await _context.Members.Where(m=>m.Username == username).Select(mi=>new MemberItemDTO
            {
                Username=mi.Username,
                Email=mi.Email,
                Phone=mi.Phone,
                Gender = mi.Gender,
            }).FirstOrDefaultAsync();
            if (userData == null)
            {
                return NotFound();
            }
            if (userData.Phone == null || userData.Gender == null)
            {
                // 如果 Phone 或 Gender 為 null，則設為 default 值
                userData.Phone = string.Empty;
                userData.Gender = false;
            }

            return Ok(userData);
        }

        // PUT: api/DessertOrders/5
        
        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateCustomerData(string username, [FromBody] MemberUpdateDTO memberUpdateDto)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            // 從資料庫找到特定的會員
            var memberToUpdate = await _context.Members.FirstOrDefaultAsync(m => m.Username == username);

            if (memberToUpdate == null)
            {
                return NotFound($"Member with username {username} not found.");
            }

            // 更新Phone和Gender
            // 在後端檢查Phone和Gender是否為null
            if (!string.IsNullOrEmpty(memberUpdateDto.Phone))
            {
                memberToUpdate.Phone = memberUpdateDto.Phone;
            }

            if (memberUpdateDto.Gender.HasValue)
            {
                memberToUpdate.Gender = memberUpdateDto.Gender.Value;
            }

            // 儲存變更到資料庫
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 可以進一步處理錯誤，例如檢查是否因為資料庫鎖定或其他原因而導致儲存失敗
                return StatusCode(500, $"Internal server error: {ex.ToString()}");
            }

            // 回傳成功訊息
            return Ok($"Member with username {username} updated successfully.");
        }

        // POST: api/DessertOrders
        [HttpPost("DessertOrders")]
        public async Task<ActionResult> PostDessertOrder([FromBody] CreateDessertOrderDTO orderDto)
        {
            // 開始事務
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 根據標識符(username)找到購物車
                    var cart = await _context.DessertCarts
                        .Include(c => c.DessertCartItems).ThenInclude(ci=>ci.Dessert).ThenInclude(x=>x.Specifications)
                        .FirstOrDefaultAsync(c => c.Username == orderDto.Username);
                    if (cart == null) throw new Exception("Cart not found");

                    // 創建訂單
                    var order = new DessertOrder
                    {
                        Id= (int)orderDto.Id,
                        MemberId= orderDto.MemberId,
                        Username = orderDto.Username,
                        DessertOrderStatusId= orderDto.DessertOrderStatusId,
                        MemberCouponId= orderDto.MemberCouponId,
                        CreateTime = DateTime.Now,
                        Recipient = orderDto.Recipient,
                        RecipientPhone = orderDto.RecipientPhone,
                        RecipientAddress = orderDto.RecipientAddress,
                        ShippingFee= orderDto.ShippingFee,
                        DessertOrderTotal = orderDto.DessertOrderTotal,
                        DeliveryMethod = orderDto.DeliveryMethod,
                        Note = orderDto.Note,
                        PayMethod = orderDto.PayMethod,
                        DiscountInfo= orderDto.DiscountInfo,


                    };
                    _context.DessertOrders.Add(order);
                    await _context.SaveChangesAsync();

                    // 創建訂單明細
                    foreach (var item in cart.DessertCartItems)
                    {
                        var orderDetail = new DessertOrderDetail
                        {
                            DessertOrderId = order.Id,
                            DessertId = item.DessertId,
                            DessertName = item.Dessert.DessertName, 
                            Quantity = item.Quantity,
                            UnitPrice = item.Specification.UnitPrice,
                            Subtotal = item.Quantity * item.Specification.UnitPrice
                        };
                        _context.DessertOrderDetails.Add(orderDetail);
                    }
                    await _context.SaveChangesAsync();

                    // 清空購物車
                    _context.DessertCartItems.RemoveRange(cart.DessertCartItems);
                    await _context.SaveChangesAsync();

                    // 提交事務
                    await transaction.CommitAsync();

                    return Ok(new { message = "Order created successfully" });
                }
                catch (Exception e)
                {
                    // 發生錯誤
                    await transaction.RollbackAsync();
                    return BadRequest(new { message = e.Message });
                }
            }
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

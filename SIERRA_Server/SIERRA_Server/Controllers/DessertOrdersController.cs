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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostDessertOrder([FromBody] CreateDessertOrderDTO orderDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 根据username查找会员
                    var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == orderDto.MemberId);
                    if (member == null)
                    {
                        return NotFound($"找不到用戶為 {orderDto.MemberId} 的會員。");
                    }

                    // 為會員創建並新增主訂單
                    var order = new DessertOrder
                    {
                        Id = orderDto.Id,
                        MemberId = member.Id,
                        //Username = orderDto.Username,
                        DessertOrderStatusId = orderDto.DessertOrderStatusId,
                        //MemberCouponId= orderDto.MemberCouponId,
                        CreateTime = DateTime.Now,
                        Recipient = orderDto.Recipient,
                        RecipientPhone = orderDto.RecipientPhone,
                        RecipientAddress = orderDto.RecipientAddress,
                        ShippingFee = orderDto.ShippingFee,
                        DessertOrderTotal = orderDto.DessertOrderTotal,
                        DeliveryMethod = orderDto.DeliveryMethod,
                        Note = orderDto.Note,
                        DiscountInfo = orderDto.DiscountInfo,
                        //PayMethod = orderDto.PaymentMethod,

                    };

                    _context.DessertOrders.Add(order);
                    await _context.SaveChangesAsync();

                    // 建立訂單明細
                    foreach (var item in orderDto.Items)
                    {
                        var orderDetail = new DessertOrderDetail
                        {
                            DessertOrderId = order.Id, // 引用新創建的訂單
                            //DessertId = item.DessertId,
                            DessertName = item.DessertName,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            Subtotal = item.Subtotal

                        };

                        _context.DessertOrderDetails.Add(orderDetail);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok($" {orderDto.MemberId} 成功建立訂單。");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
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

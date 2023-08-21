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
    public class LessonOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LessonOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LessonOrders
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<LessonOrder>>> GetLessonOrders()
        //{
        //  if (_context.LessonOrders == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.LessonOrders.ToListAsync();
        //}

        // GET: api/LessonOrders/GetCustomerData
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberItemDTO>> GetCustomerData(string? username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var userData = await _context.Members.Where(m => m.Username == username).Select(mi => new MemberItemDTO
            {
                Username = mi.Username,
                Email = mi.Email,
                Phone = mi.Phone,
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

        // PUT: api/LessonOrders/ UpdateCustomerData
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

        //POST: api/LessonOrders
       [HttpPost]
        public async Task<IActionResult> CreateOrderForMember([FromBody] CreateLessonOrderDTO dto)
        {
            // 驗證會員是否存在
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == dto.MemberId);
            if (member == null)
            {
                return NotFound(new { Message = "Member not found" });
            }
            // 計算Subtotal
            var calculatedSubtotal = dto.LessonUnitPrice * dto.NumberOfPeople;
            // 創建訂單
            var order = new LessonOrder
            {
                Id=dto.Id,
                MemberId = dto.MemberId,
                Username = dto.Username,
                LessonOrderStatusId=3,
                LessonOrderTotal = dto.LessonOrderTotal,
                PayMethod = dto.PayMethod,
                Note = dto.Note,
                CreateTime = DateTime.Now
            };

            _context.LessonOrders.Add(order);
            await _context.SaveChangesAsync();

            var orderDetail = new LessonOrderDetail
            {
                LessonOrderId = order.Id,
                LessonId = dto.LessonId,
                LessonTitle = dto.LessonTitle,
                NumberOfPeople = dto.NumberOfPeople,
                LessonUnitPrice = dto.LessonUnitPrice,
                Subtotal = calculatedSubtotal
            };

            _context.LessonOrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order created successfully for member", OrderId = order.Id });
        }
    }

    // DELETE: api/LessonOrders/5
    //[HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteLessonOrder(int id)
    //    {
    //        if (_context.LessonOrders == null)
    //        {
    //            return NotFound();
    //        }
    //        var lessonOrder = await _context.LessonOrders.FindAsync(id);
    //        if (lessonOrder == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.LessonOrders.Remove(lessonOrder);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool LessonOrderExists(int id)
    //    {
    //        return (_context.LessonOrders?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}

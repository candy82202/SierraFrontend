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
        public async Task<ActionResult> PostLessonOrder([FromBody] CreateLessonOrderDTO orderDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {

                try
                {
                    // 根據標識符(username)
                    var lesson = await _context.LessonOrders
                        .Include(l => l.LessonOrderDetails).ThenInclude(li=>li.Lesson)
                        .FirstOrDefaultAsync(c => c.Username == orderDto.Username);
                    if (lesson == null) throw new Exception("lesson not found");

                    // 創建訂單
                    var order = new LessonOrder
                    {
                        Id = (int)orderDto.Id,
                        MemberId = (int)orderDto.MemberId,
                        Username = orderDto.Username,
                        LessonOrderStatusId = 3,
                        CreateTime = DateTime.Now,
                        LessonOrderTotal = orderDto.LessonOrderTotal,
                        Note = orderDto.Note,
                        PayMethod = orderDto.PayMethod,

                    };
                    _context.LessonOrders.Add(order);
                    await _context.SaveChangesAsync();

                    //創建訂單明細
                    foreach (var item in lesson.LessonOrderDetails)
                    {
                        var orderDetail = new LessonOrderDetail
                        {
                            LessonOrderId = order.Id,
                            LessonTitle = item.LessonTitle,
                            LessonId = item.LessonId,
                            NumberOfPeople = orderDto.ActualCapacity,
                            LessonUnitPrice = item.LessonUnitPrice,
                            Subtotal = orderDto.LessonUnitPrice * orderDto.ActualCapacity,
                        };
                        _context.LessonOrderDetails.Add(orderDetail);
                    }
                    await _context.SaveChangesAsync();

                    //var orderDetail = new LessonOrderDetail
                    //{
                    //    LessonOrderId = order.Id,
                    //    LessonTitle = orderDto.LessonTitle, 
                    //    LessonId = orderDto.LessonId, 
                    //    NumberOfPeople = orderDto.ActualCapacity, 
                    //    LessonUnitPrice = orderDto.LessonUnitPrice,
                    //    Subtotal = orderDto.LessonUnitPrice * orderDto.ActualCapacity 
                    //};
                    //_context.LessonOrderDetails.Add(orderDetail);
                    //await _context.SaveChangesAsync();


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

            // DELETE: api/LessonOrders/5
            [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLessonOrder(int id)
        {
            if (_context.LessonOrders == null)
            {
                return NotFound();
            }
            var lessonOrder = await _context.LessonOrders.FindAsync(id);
            if (lessonOrder == null)
            {
                return NotFound();
            }

            _context.LessonOrders.Remove(lessonOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessonOrderExists(int id)
        {
            return (_context.LessonOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

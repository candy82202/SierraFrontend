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
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<DessertOrder>>> GetDessertOrders()
        //{
        //  if (_context.DessertOrders == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.DessertOrders.ToListAsync();
        //}

        //GET: api/DessertOrders/GetCustomerData
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

        // PUT: api/DessertOrders/ UpdateCustomerData

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
        [HttpPost]
        public async Task<ActionResult> PostDessertOrder([FromBody] CreateDessertOrderDTO orderDto)
        {
            
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
                        DessertOrderStatusId= 1,
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
                            SpecificationId=item.SpecificationId,
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

        //GET: api/DessertOrders
        [HttpPost("GetCustomerOrder")]
        public async Task<IActionResult> GetCustomerOrder(GetCustomerOrderDTO dto)
        {
            //根據username找到所有的訂單及訂單狀態
            //最新訂單排最前
            if (dto.Username == null)
            {
                return NotFound();
            }
            var userOrder = await _context.DessertOrders.Include(c=>c.MemberCoupon).Include(od => od.DessertOrderDetails).Include(o => o.DessertOrderStatus)
        .Where(o => o.Username == dto.Username)
        .Select(x => new GetCustomerOrderDTO
        {
            Id = x.Id,
            Username = x.Username,
            CreateTime = x.CreateTime,
            StatusName = x.DessertOrderStatus.StatusName,
            DeliveryMethod = x.DeliveryMethod,
            DessertOrderTotal = x.DessertOrderTotal,
            Recipient = x.Recipient,
            RecipientPhone = x.RecipientPhone,
            RecipientAddress = x.RecipientAddress,
            ShippingFee = x.ShippingFee,
            PayMethod = x.PayMethod,
            Note = x.Note,
            CouponName=x.MemberCoupon.CouponName,
            DiscountInfo = x.DiscountInfo,
            DessertOrderDetails = (List<ItemDto>)x.DessertOrderDetails.Select(n => new ItemDto
            {
                DessertName = n.DessertName,
                Quantity = n.Quantity,
                UnitPrice = n.UnitPrice,
                Subtotal = n.Subtotal,
            })

        }).OrderByDescending(o => o.CreateTime).ToListAsync();

            if (userOrder == null)
            {
                return NotFound();
            }
            return Ok(userOrder);
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

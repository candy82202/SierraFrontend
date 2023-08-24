using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SIERRA_Server.Models.EFModels;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Net.Http;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECPayLessonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ECPayLessonController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> EcPayCheckOut([FromBody] UserRequest request)
        {
            
            //根據提供的username查找最新一筆訂單資訊
            var latestOrder = await _context.LessonOrders
                                            .Where(o => o.Username == request.Username)
                                            .OrderByDescending(o => o.CreateTime)
                                            .FirstOrDefaultAsync();
            var lessonOrderTotal = latestOrder.LessonOrderTotal;

           

            //根據最新訂單的Id獲取所有相應的DessertOrderDetails資料
            var lessonDetails = await _context.LessonOrderDetails.Where(d => d.LessonOrderId == latestOrder.Id).ToListAsync();

            if (!lessonDetails.Any())
            {
                return BadRequest("No lesson details found for the order.");
            }

            //將所有的lessonTitle使用#組合成一個字符串
            var combinedlessonTitles = string.Join("#", lessonDetails.Select(d => d.LessonTitle));

            var orderId = Guid.NewGuid().ToString("N").Substring(0, 5);


            //需填入你的網址
            var website = "https://8c53-2001-b400-e290-8861-387b-291-d5c6-fbc0.ngrok.io ";
            var order = new Dictionary<string, string>
            {
                
                {"MerchantID","3002607" },
                {"MerchantTradeNo",orderId},
                {"MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                {"TotalAmount",lessonOrderTotal.ToString() },
                {"TradeDesc", "在這個課程中，您將學習手工製作法式馬卡龍。從馬卡龍的製作、內餡的調配到最後的裝飾，讓您的法式馬卡龍美味可口、色彩豐富。"},
                {"ItemName",combinedlessonTitles },
                {"ReturnURL", "https://8c53-2001-b400-e290-8861-387b-291-d5c6-fbc0.ngrok.io"},
                { "ClientBackURL", "http://localhost:5501/Order.html"},
                { "EncryptType",  "1"},
                {"ChoosePayment" ,"Credit"},
                {"PaymentType", "aio"},

            };
            // 檢查碼計算
            string hashKey = "pwFHCqoQZGmho4w6";
            string hashIV = "EkRm7iFT261dpevs";
            string rawString = $"HashKey={hashKey}&" + string.Join("&", order.OrderBy(x => x.Key).Select(x => $"{x.Key}={x.Value}")) + $"&HashIV={hashIV}";
            string urlEncode = WebUtility.UrlEncode(rawString).ToLower();
            string checkMacValue = BitConverter.ToString(new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(urlEncode))).Replace("-", "").ToUpper();

            // 加入 CheckMacValue
            order.Add("CheckMacValue", checkMacValue);
            return Ok(order);
        }
    }
  
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class EcPayController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EcPayController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> EcPayCheckOut()
        {
            // 從DessertOrders獲取第一筆資料
            var firstOrder = await _context.DessertOrders.FirstOrDefaultAsync();
            if (firstOrder == null)
            {
                return BadRequest("No order found.");
            }
            var dessertOrderTotal = firstOrder.DessertOrderTotal;

            var firstDessert = await _context.DessertOrderDetails.FirstOrDefaultAsync();
            if (firstDessert == null)
            {
                return BadRequest("No order found.");
            }
            var dessertName = firstDessert.DessertName;

           

            var orderId = Guid.NewGuid().ToString("N").Substring(0, 5);
            //需填入你的網址
            var website = "https://8c53-2001-b400-e290-8861-387b-291-d5c6-fbc0.ngrok.io ";
            var order = new Dictionary<string, string>
        {
                //{"HashKey","pwFHCqoQZGmho4w6"},
                //{"HashIV" ,"EkRm7iFT261dpevs"},
                {"MerchantID","3002607" },
            {"MerchantTradeNo",orderId},
            {"MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                {"TotalAmount",dessertOrderTotal.ToString() },
                {"TradeDesc", "香草輕乳酪選用的是馬達加斯加的天然香草籽切開蛋糕後會清晰地看到香草籽佈滿其中"},
                {"ItemName",dessertName },
                {"ReturnURL", "https://8c53-2001-b400-e290-8861-387b-291-d5c6-fbc0.ngrok.io"},
                 //{ "OrderResultURL", ""},
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

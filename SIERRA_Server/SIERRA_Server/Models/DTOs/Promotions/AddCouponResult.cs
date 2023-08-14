namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class AddCouponResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public static AddCouponResult Success(string message)
        =>new AddCouponResult {IsSuccess=true, Message = message };
        public static AddCouponResult Fail(string message)
        => new AddCouponResult { IsSuccess = false, Message = message };
    }
}
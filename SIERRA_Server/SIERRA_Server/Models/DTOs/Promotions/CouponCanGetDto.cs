namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class CouponCanGetDto
    {
        public string CouponName { get; set; }
        public string ExpireAt { get; set; }
        public string ApplyTo { get; set; }
        public string ApplyToDetail { get; set; }
        public int CouponType { get; set; }
        public int CouponId { get; set; }
    }
}
namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class MemberCouponHasUsedDto
    {
        public int MemberCouponId { get; set; }
        public string CouponName { get; set;}
        public DateTime UsedAt { get; set; }
        public string UsedAtText
        {
            get
            {
                return this.UsedAt.ToString("yyyy/MM/dd");
            }
        }
    }
}
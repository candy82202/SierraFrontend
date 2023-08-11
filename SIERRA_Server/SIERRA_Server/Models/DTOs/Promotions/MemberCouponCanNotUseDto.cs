namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class MemberCouponCanNotUseDto
    {
        public int MemberCouponId { get; set; }
        public string CouponName { get; set; }

        public DateTime StartAt { get; set; }
        public string StartAtText
        {
            get
            {
                return StartAt.ToString("yyyy/MM/dd");
            }
        }
        public string ApplyTo { get; set; }
        public int CouponType { get; set; }
    }
}

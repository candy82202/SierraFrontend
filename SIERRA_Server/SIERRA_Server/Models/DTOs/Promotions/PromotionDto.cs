namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class PromotionDto
    {
        public int? CouponId { get; set; }
        public string PromotionName { get; set; }
        public string PromotionImage { get; set; }
        public string Description { get; set; }
        public DateTime LaunchAt { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string TimeRange { 
            get
            {
                return $"{this.StartAt.ToString("yyyy-MM-dd HH:mm:ss")} ~ {this.EndAt.ToString("yyyy-MM-dd HH:mm:ss")}";
            }
        }
    }
}
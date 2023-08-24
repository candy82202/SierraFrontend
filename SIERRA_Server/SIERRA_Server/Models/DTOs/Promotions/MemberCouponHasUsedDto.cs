namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class MemberCouponHasUsedDto
    {
        public int MemberCouponId { get; set; }
        public string CouponName { get; set;}
        public DateTime UsedAt { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime ExpireAt { get; set; }
		public string GetTime
		{
			get
			{
				return CreateAt.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}
		public DateTime StartAt { get; set; }
		public string CanUseTimeRange
		{
			get
			{
				return $"{StartAt.ToString("yyyy-MM-dd HH:mm:ss")} - {ExpireAt.ToString("yyyy-MM-dd HH:mm:ss")}";
			}
		}
		public string ApplyTo { get; set; }
		public string ApplyToDetail { get; set; }
		public string UsedAtText
        {
            get
            {
                return this.UsedAt.ToString("yyyy/MM/dd");
            }
        }
        public int CouponType { get; set; }
    }
}
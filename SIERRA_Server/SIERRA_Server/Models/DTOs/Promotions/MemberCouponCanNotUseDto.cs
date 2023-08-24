namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class MemberCouponCanNotUseDto
    {
        public int MemberCouponId { get; set; }
        public string CouponName { get; set; }

        public DateTime StartAt { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime ExpireAt { get; set; }
		public string GetTime
		{
			get
			{
				return CreateAt.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}
		public string CanUseTimeRange
		{
			get
			{
				return $"{StartAt.ToString("yyyy-MM-dd HH:mm:ss")} - {ExpireAt.ToString("yyyy-MM-dd HH:mm:ss")}";
			}
		}
		public string StartAtText
        {
            get
            {
                return StartAt.ToString("yyyy/MM/dd");
            }
        }
        public string ApplyTo { get; set; }
		public string ApplyToDetail { get; set; }
		public int CouponType { get; set; }
    }
}

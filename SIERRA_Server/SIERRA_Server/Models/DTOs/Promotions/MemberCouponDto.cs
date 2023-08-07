using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class MemberCouponDto
    {
        public int MemberCouponId { get; set; }
        public string CouponName { get; set; }
        public DateTime ExpireAt { get; set; }
        public string RemainingTime
        {
            get
            {
                var result = ExpireAt - DateTime.Now;
                if (result.TotalDays >= 365)
                {
                    return $"一年以上";
                }
                else if (result.TotalDays < 365 && result.TotalDays >= 30)
                {
                    return $"一個月以上";
                }
                else if (result.TotalDays >= 14 && result.TotalDays < 30)
                {
                    return $"兩周以上";
                }
                else if (result.TotalDays < 14 && result.TotalDays >= 7)
                {
                    return $"一周以上";
                }
                else if (result.TotalDays < 7 && result.TotalDays >= 1)
                {
                    return $"{Math.Floor(result.TotalDays)}天";
                }
                else if (result.TotalDays < 1 && result.TotalHours >= 1)
                {
                    return $"{Math.Floor(result.TotalHours)}小時";
                }
                else if (result.TotalHours < 1 && result.TotalMinutes >= 1)
                {
                    return $"{Math.Floor(result.TotalMinutes)}分鐘";
                }
                else
                {
                    return "不到一分鐘";
                }
            }
        }
        public string ApplyTo { get; set; }
        public int CouponId { get; set; }

	}
}
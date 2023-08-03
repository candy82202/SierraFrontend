namespace SIERRA_Server.Models.Infra
{
    public class ResultForCheck
    {
        public int CouponId { get; set; }
        public bool HaveSame { get; set; }
        public ResultForCheck(int couponId,bool haveSame)
        {
            CouponId = couponId;
            HaveSame = haveSame;
        }
    }
}
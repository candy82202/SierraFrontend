using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Infra.Promotions
{
    public class TestResult
    {
        public TestResult(int score, CouponSetting setting)
        {
            Score = score;
            Setting = setting;
        }
        public CouponSetting Setting { get; set; }
        public int Score { get; set; }
    }
}
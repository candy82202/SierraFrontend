using SIERRA_Server.Models.Infra.Promotions;

namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class WeeklyGameResult
    {
        public AddCouponResult Result { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<SuggestProductDto> SuggestProducts { get; set; }
    }
}
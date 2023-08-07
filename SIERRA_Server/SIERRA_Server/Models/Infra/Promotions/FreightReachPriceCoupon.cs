using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Infra.Promotions
{
    public class FreightReachPriceCoupon:ICoupon
    {
        public int NeededPrice { get; set; }
        public FreightReachPriceCoupon(int neededPrice)
        {
            NeededPrice = neededPrice;
        }

        public string Calculate(IEnumerable<DessertCartItem> items)
        {
            var totalPrice = items.Select(i => i.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
            ? Math.Round((decimal)i.Specification.UnitPrice * ((decimal)i.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : i.Specification.UnitPrice).Sum();
            if (totalPrice >= this.NeededPrice)
            {
                return "-60";
            }
            else return "無法使用此優惠券";
        }
    }
}

using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Infra.Promotions
{
    public class PercentReachPriceCoupon : ICoupon
    {
        public int NeededPrice { get; set; }
        public int Discount { get; set; }
        public PercentReachPriceCoupon(int neededPrice,int discount)
        {
            NeededPrice = neededPrice;
            Discount = discount;
        }
        public int Calculate(IEnumerable<DessertCartItem> items)
        {
            var totalPrice = items.Select(i => i.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
            ? Math.Round((decimal)i.Specification.UnitPrice * ((decimal)i.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : i.Specification.UnitPrice).Sum();
            if(totalPrice>= this.NeededPrice)
            {
                var result = Math.Round((decimal)totalPrice * (decimal)this.Discount / 100, 0, MidpointRounding.AwayFromZero);
                var discountValue = result - totalPrice;
                return (int)discountValue;
            }
            else return 0;
        }
    }
}

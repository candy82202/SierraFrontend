using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Infra.Promotions
{
	public class ReduceCoupon : ICoupon
	{
        public int ReducePrice { get; set; }
        public ReduceCoupon(int reducePrice)
        {
			ReducePrice=reducePrice;
		}
        public string Calculate(IEnumerable<DessertCartItem> items)
		{
			var totalPrice = items.Select(i => i.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
			? Math.Round((decimal)i.Specification.UnitPrice * ((decimal)i.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : i.Specification.UnitPrice).Sum();
			var discountPrice = totalPrice - ReducePrice;
			var discountValue = discountPrice - totalPrice;

            return $"符合條件:{discountValue}";
		}
	}
}

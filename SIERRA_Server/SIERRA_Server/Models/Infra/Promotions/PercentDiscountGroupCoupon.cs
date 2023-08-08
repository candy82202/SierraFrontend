using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Infra.Promotions
{
    public class PercentDiscountGroupCoupon:ICoupon
    {
        public IEnumerable<int> DessertsIdInDiscountGroup { get; set; }
        public int Discount { get; set; }
        public PercentDiscountGroupCoupon(IEnumerable<int> ids,int discount)
        {
            DessertsIdInDiscountGroup = ids;
            Discount = discount;
        }

        public int Calculate(IEnumerable<DessertCartItem> items)
        {
            if (items.Any(i => this.DessertsIdInDiscountGroup.Contains(i.DessertId)))
            {
                var itemsInDiscountGroup = items.Where(i => DessertsIdInDiscountGroup.Contains(i.DessertId));
                var highestOne = itemsInDiscountGroup.MaxBy(i=> i.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
            ? Math.Round(i.Specification.UnitPrice * ((decimal)i.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : i.Specification.UnitPrice);
                var highestOnePrice = highestOne.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
            ? Math.Round(highestOne.Specification.UnitPrice * ((decimal)highestOne.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : highestOne.Specification.UnitPrice;
                var discountPrice = Math.Round(highestOnePrice * ((decimal)Discount / 100), 0, MidpointRounding.AwayFromZero);
                var discountValue = discountPrice - highestOnePrice;
                return (int)discountValue;
            }
            else return 0;
        }
    }
}

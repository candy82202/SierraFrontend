using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using System.Linq;

namespace SIERRA_Server.Models.Infra.Promotions
{
	public class PercentReachCountCoupon:ICoupon
	{
        public IEnumerable<int> Ids { get; set; }
        public int NeededCount { get; set; }
        public int Discount { get; set; }
        public PercentReachCountCoupon(IEnumerable<int> ids,int neededCount,int discount)
        {
            Ids = ids;
            NeededCount = neededCount;
            Discount = discount;
        }

		public int Calculate(IEnumerable<DessertCartItem> items)
		{
			var count = items.Where(i=> Ids.Contains(i.DessertId)).Select(i=>i.Quantity).Sum();
            if(count >= NeededCount)
            {
                var itemsWithPrice = items.Where(i => Ids.Contains(i.DessertId))
                                          .Select(i => new {
                                                DessertId=i.DessertId,
                                                Count = i.Quantity,
                                                Price = i.Dessert.Discounts.Any(d=>d.StartAt<DateTime.Now&&d.EndAt>DateTime.Now)?Math.Round(i.Specification.UnitPrice*(decimal)i.Dessert.Discounts.First().DiscountPrice/100,0,MidpointRounding.AwayFromZero): i.Specification.UnitPrice
                                            })
                                          .OrderByDescending(i=>i.Price)
                                          .ToArray();

				List<int> DiscountItems= new List<int>();
				for (int i=0; i< itemsWithPrice.Length; i++)
                {
					
					for (int j = 1; j <= itemsWithPrice[i].Count; j++)
                    {
						DiscountItems.Add((int)itemsWithPrice[i].Price);
                        if(DiscountItems.Count()>= NeededCount)
                        {
                            break;
                        }
					}
					if (DiscountItems.Count() >= NeededCount)
					{
						break; // 中斷外層迴圈的執行
					}
				}
                var totalPrice = DiscountItems.Sum();
                var discountPrice = Math.Round( totalPrice * (decimal)Discount / 100,0,MidpointRounding.AwayFromZero);
                var discountValue = discountPrice - totalPrice;
                return (int)discountValue;
			}
			else return 0;
		}
	}
}

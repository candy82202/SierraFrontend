using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Infra.Promotions
{
	public class ReduceReachCountCoupon:ICoupon
	{
        public IEnumerable<int> Ids { get; set; }
        public int ReducePrice { get; set; }
        public int NeededCount { get; set; }
        public ReduceReachCountCoupon(IEnumerable<int> ids,int reducePrice,int neededCount)
        {
            Ids = ids;
            ReducePrice = reducePrice;
            NeededCount = neededCount;
        }

		public int Calculate(IEnumerable<DessertCartItem> items)
		{
			var count = items.Where(i => Ids.Contains(i.DessertId)).Select(i => i.Quantity).Sum();
            if (count >= NeededCount)
            {
                return -ReducePrice;
            }
            else return 0;
		}
	}
}

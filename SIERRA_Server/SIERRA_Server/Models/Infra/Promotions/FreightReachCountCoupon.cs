using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Infra.Promotions
{
	public class FreightReachCountCoupon:ICoupon
	{
        public IEnumerable<int> Ids { get; set; }
        public int NeededCount { get; set; }
        public FreightReachCountCoupon(IEnumerable<int>ids,int neededCount)
        {
            Ids = ids;
            NeededCount = neededCount;
        }

		public int Calculate(IEnumerable<DessertCartItem> items)
		{
			var count = items.Where(i => Ids.Contains(i.DessertId)).Select(i => i.Quantity).Sum();
			if (count >= NeededCount)
			{
				return -60;
			}
			else return 0;
		}
	}
}

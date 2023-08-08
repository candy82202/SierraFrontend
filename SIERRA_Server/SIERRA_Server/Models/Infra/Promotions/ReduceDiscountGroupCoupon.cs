using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Infra.Promotions
{
	public class ReduceDiscountGroupCoupon : ICoupon
	{
        public IEnumerable<int> Ids { get; set; }
        public int ReducePrice { get; set; }
        public ReduceDiscountGroupCoupon(IEnumerable<int> ids,int reducePrice)
        {
            Ids = ids;
            ReducePrice = reducePrice;
        }

		public int Calculate(IEnumerable<DessertCartItem> items)
		{
			if (items.Any(i => this.Ids.Contains(i.DessertId)))
			{
				return -ReducePrice;
			}
			else return 0;
		}
	}
}

using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Infra.Promotions
{
	public class FreightDiscountGroupCoupon:ICoupon
	{
        public IEnumerable<int> Ids { get; set; }
        public FreightDiscountGroupCoupon(IEnumerable<int> ids)
        {
            Ids = ids;
        }

		public int Calculate(IEnumerable<DessertCartItem> items)
		{
			if (items.Any(i => this.Ids.Contains(i.DessertId)))
			{
				return -60;
			}
			else return 0;
		}
	}
}

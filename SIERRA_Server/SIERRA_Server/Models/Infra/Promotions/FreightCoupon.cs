using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Infra.Promotions
{
    public class FreightCoupon : ICoupon
    {
        public int Calculate(IEnumerable<DessertCartItem> items)
        {
            
            return -60;
        }
    }
}

using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Interfaces
{
    public interface ICoupon
    {
        int Calculate(IEnumerable<DessertCartItem> items);
    }
}

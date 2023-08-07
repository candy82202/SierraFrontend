using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Interfaces
{
    public interface ICoupon
    {
        string Calculate(IEnumerable<DessertCartItem> items);
    }
}

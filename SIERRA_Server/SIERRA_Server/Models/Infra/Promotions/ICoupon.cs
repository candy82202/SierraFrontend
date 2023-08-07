using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Infra.Promotions
{
	public interface ICoupon
	{
		string Calculate(IEnumerable<DessertCartItem> items);
	}
}

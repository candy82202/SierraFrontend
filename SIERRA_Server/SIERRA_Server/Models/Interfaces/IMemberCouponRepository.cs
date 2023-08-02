using SIERRA_Server.Models.DTOs;

namespace SIERRA_Server.Models.Interfaces
{
	public interface IMemberCouponRepository
	{
	     Task<IEnumerable<MemberCouponDto>>GetUsableCoupon(int? MemberId);
	}
}

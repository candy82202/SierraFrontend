using SIERRA_Server.Models.DTOs;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Services
{
	public class MemberCouponService
	{
		private IMemberCouponRepository _repo;

        public MemberCouponService(IMemberCouponRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MemberCouponDto>> GetUsableCoupon(int? MemberId)
        {
            var coupons = await _repo.GetUsableCoupon(MemberId);
            return coupons;
        }

	}
}

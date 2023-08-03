using SIERRA_Server.Models.DTOs.Promotions;
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

        public async Task<IEnumerable<MemberCouponDto>> GetUsableCoupon(int? memberId)
        {
            var coupons = await _repo.GetUsableCoupon(memberId);
            return coupons;
        }

        public async Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? memberId)
        {
            var coupons = await _repo.GetCouponCanNotUseNow(memberId);
            return coupons;
        }

        public async Task<string> GetCouponByCode(int memberId, string code)
        {
            var isExist = await _repo.CheckCouponExist(code);
            if (!isExist)
            {
                return "查無此優惠碼";
            }
            else
            {
                var result =await _repo.CheckHaveSame(memberId, code);
                if (result.HaveSame)
                {
                    return "已領取過此優惠券";
                }
                else
                {
                    MemberCouponCreateDto dto = new MemberCouponCreateDto();
                    dto.CouponId = result.CouponId;
                    dto.MemberId = memberId;
                    return await _repo.GetCouponByCode(dto);
                }
            }
            
        }

        public async Task<IEnumerable<MemberCouponHasUsedDto>> GetCouponHasUsed(int? memberId)
        {
            var coupons = await _repo.GetCouponHasUsed(memberId);
            return coupons;
        }

        public async Task<IEnumerable<MemberCouponDto>> GetCouponMeetCriteria(int memberId)
        {
            var coupons = await _repo.GetCouponMeetCriteria(memberId);
            return coupons;
        }
    }
}

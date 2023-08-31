using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SIERRA_Server.Models.Repository.DPRepository
{
    public class PromotionDPRepository : IPromotionRepository
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly string _connStr;
        public PromotionDPRepository(AppDbContext db,IConfiguration config)
        {
            _db = db;
            _config = config;
            _connStr = config.GetConnectionString("Sierra");
        }
        public async Task<AddCouponResult> GetPromotionCoupon(int memberId, int couponId)
        {

            using var conn = new SqlConnection(_connStr);
            var couponQuery = @"
    SELECT CouponName, EndAt
    FROM Coupons
    WHERE CouponId = @CouponId";

            var insertQuery = @"
    INSERT INTO MemberCoupons (MemberId, CouponId, CouponName, CreateAt, ExpireAt)
    VALUES (@MemberId, @CouponId, @CouponName, @CreateAt, @ExpireAt)";
            await conn.OpenAsync();
            var coupon = await conn.QuerySingleOrDefaultAsync<Coupon>(couponQuery, new { CouponId = couponId });
            if(coupon == null)
            {
                return AddCouponResult.Fail("查無此優惠券");
            }
            var memberCoupon = new MemberCoupon()
            {
                MemberId = memberId,
                CouponId = couponId,
                CouponName = coupon.CouponName,
                CreateAt = DateTime.Now,
                ExpireAt = (DateTime)coupon.EndAt
            };
            await conn.ExecuteAsync(insertQuery, memberCoupon);
            return AddCouponResult.Success(memberCoupon.CouponName);
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsNow()
        {
            using var conn = new SqlConnection(_connStr);
            var query = @"SELECT *
FROM Promotions
WHERE LaunchAt < GETDATE() AND EndAt > GETDATE()";
            await conn.OpenAsync();

            //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
            //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
            var queryResult = await conn.QueryAsync<Promotion>(query);
            return queryResult;
        }

        public async Task<bool> HasGottenCoupon(int memberId, int couponId)
        {
            using var conn = new SqlConnection(_connStr);
            var query = @"
    SELECT *
    FROM MemberCoupons
    WHERE MemberId = @MemberId";

            var parameters = new { MemberId = memberId };
            await conn.OpenAsync();
            var memberCoupons = await conn.QueryAsync<MemberCoupon>(query, parameters);

            var sameCoupon = memberCoupons.FirstOrDefault(mc => mc.CouponId == couponId);

            if (sameCoupon == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsPromotionCoupon(int couponId)
        {
            using var conn =new SqlConnection(_connStr);
            var query = @"
SELECT *
FROM Coupons
WHERE CouponId = @CouponId";
            var parameters = new { CouponId = couponId };
            await conn.OpenAsync();
            var coupon = await conn.QuerySingleOrDefaultAsync<Coupon>(query, parameters);
            if(coupon == null || coupon.CouponCategoryId != 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

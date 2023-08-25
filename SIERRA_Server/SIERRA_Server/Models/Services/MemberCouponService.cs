using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SIERRA_Server.Models.DTOs.Promotions;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Exts;
using SIERRA_Server.Models.Infra.Promotions;
using SIERRA_Server.Models.Interfaces;
using System.Linq;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

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
            return coupons.Select(mc => mc.ToMemberCouponDto());
        }

        public async Task<IEnumerable<MemberCouponCanNotUseDto>> GetCouponCanNotUseNow(int? memberId)
        {
            var coupons = await _repo.GetCouponCanNotUseNow(memberId);
            return coupons.Select(mc => mc.ToMemberCouponCanNotUseDto());
        }

        public async Task<AddCouponResult> GetCouponByCode(int memberId, string code)
        {
            var isExist = await _repo.CheckCouponExist(code);
            if (!isExist)
            {
                return AddCouponResult.Fail("查無此優惠碼");
            }
            else
            {
                var result =await _repo.CheckHaveSame(memberId, code);
                if (result.HaveSame)
                {
                    return AddCouponResult.Fail("已領取過此優惠券");
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

        public async Task<IEnumerable<MemberCouponCanUseDto>> GetCouponMeetCriteria(int memberId)
        {
            var coupons = await DoThisToGetCouponMeetCriteria(memberId);
            var result = coupons.Select(c => c.ToMemberCouponCanUseDto());
            return result;
        }

        public async Task<IEnumerable<IneligibleMemberCouponDto>> GetIneligibleCoupon(int memberId)
        {
            var coupons = await _repo.GetUsableCoupon(memberId);
            var usableCoupons = await DoThisToGetCouponMeetCriteria(memberId);
			var cart = await _repo.GetDessertCart(memberId);
			var ineligibleCoupons = coupons.Except(usableCoupons).Select(mc => mc.ToIneligibleMemberCouponDto(cart));
            return ineligibleCoupons;
        }
        public async Task<IEnumerable<CouponCanGetDto>> GetCouponCanGet(int memberId)
        {
            var promotionCoupons = await _repo.GetPromotionCoupons();
            var promotionCouponsId = promotionCoupons.Select(c => c.CouponId);
            var memberCouponsId = await _repo.GetAllMemberPromotionCoupon(memberId);
            var couponCanGet = promotionCoupons.ToList();
            foreach (int couponId in memberCouponsId)
            {
                if (promotionCouponsId.Contains(couponId))
                {
                    var couponShouldBeRemove = couponCanGet.Find(c => c.CouponId == couponId);
                    couponCanGet.Remove(couponShouldBeRemove);
                }
            }

            return couponCanGet.Select(c=>c.ToCouponCanGetDto());
        }
		public  bool IsMemberExist(int memberId)
		{
            var result = _repo.IsMemberExist(memberId);
            return result;
		}
		

		public bool IsMemberCouponExist(int memberCouponId)
		{
			var result = _repo.IsMemberCouponExist(memberCouponId);
			return result;
		}
		public bool IsThisMemberHaveThisCoupon(int memberId, int memberCouponId)
		{
			var result = _repo.IsThisMemberHaveThisCoupon(memberId, memberCouponId);
			return result;
		}
		public async Task<int> UseCouponAndCalculateDiscountPrice(int memberId,int memberCouponId)
		{
            var memberCoupon =await _repo.GetMemberCouponById(memberCouponId);
			var cart = await _repo.GetDessertCart(memberId);
			var cartItems = cart.DessertCartItems.ToList();

			ICoupon coupon;
			//根據coupon中的欄位new不同的類別
			if (memberCoupon.DiscountGroupId == null)
			{
				if(memberCoupon.LimitType ==null)
				{
					if (memberCoupon.DiscountType == 1)
					{
						 coupon = new ReduceCoupon((int)memberCoupon.DiscountValue);
					}else if(memberCoupon.DiscountType == 2)
					{
						 coupon =new PercentCoupon((int)memberCoupon.DiscountValue);
					}
					else
					{
						 coupon = new FreightCoupon();
					}
				}
				else
				{
					if (memberCoupon.DiscountType == 1)
					{
						 coupon = new ReduceReachPriceCoupon((int)memberCoupon.LimitValue, (int)memberCoupon.DiscountValue);
					}
					else if(memberCoupon.DiscountType == 2)
					{
						 coupon = new PercentReachPriceCoupon((int)memberCoupon.LimitValue, (int)memberCoupon.DiscountValue);
					}
					else
					{
						 coupon = new FreightReachPriceCoupon((int)memberCoupon.LimitValue);
					}
				}
			}
			else
			{
				var dessertsInDiscountGroupId = memberCoupon.DiscountGroup.DiscountGroupItems.Select(dgi => dgi.DessertId);
				if (memberCoupon.LimitType == null)
				{
					if (memberCoupon.DiscountType == 1)
					{
						 coupon = new ReduceDiscountGroupCoupon(dessertsInDiscountGroupId,(int)memberCoupon.DiscountValue);
					}
					else if (memberCoupon.DiscountType == 2)
					{
						 coupon = new PercentDiscountGroupCoupon(dessertsInDiscountGroupId, (int)memberCoupon.DiscountValue);
					}
					else
					{
						 coupon = new FreightDiscountGroupCoupon(dessertsInDiscountGroupId);
					}
				}
				else
				{
					if (memberCoupon.DiscountType == 1)
					{
						 coupon = new ReduceReachCountCoupon(dessertsInDiscountGroupId,(int)memberCoupon.DiscountValue,(int)memberCoupon.LimitValue);
					}
					else if (memberCoupon.DiscountType == 2)
					{
						 coupon = new PercentReachCountCoupon(dessertsInDiscountGroupId,(int)memberCoupon.LimitValue, (int)memberCoupon.DiscountValue);
					}
					else
					{
						 coupon = new FreightReachCountCoupon(dessertsInDiscountGroupId, (int)memberCoupon.LimitValue);
					}
				}
			}
			//ICoupon coupon = new ReduceReachCountCoupon(dessertsInDiscountGroupId, (int)memberCoupon.DiscountValue, (int)memberCoupon.LimitValue);
			var price = coupon.Calculate(cartItems);
			var totalPrice = cartItems.Select(i => i.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
			? Math.Round(i.Specification.UnitPrice * ((decimal)i.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : i.Specification.UnitPrice).Sum();
			var result = Math.Abs(price)> totalPrice? (int)-totalPrice:price;
			if (result != 0)
			{
				_repo.RecordCouponInCart(cart, memberCouponId,result);
			}
			return result;
		}
        public  bool HasCouponBeenUsed(int memberCouponId)
        {
			var result = _repo.HasCouponBeenUsed(memberCouponId);
			return result;
        }
		public bool IsPromotionCouponAndReady(int memberCouponId)
		{
			var result = _repo.IsPromotionCouponAndReady(memberCouponId);
			return result;
		}
		public async Task<AddCouponResult> PlayDailyGame(int memberId)
		{
			if(!_repo.IsMemberExist(memberId))
			{
				return AddCouponResult.Fail("查無此優惠券");
			}
			if (await _repo.HasPlayedGame(memberId))
			{
				return AddCouponResult.Fail("今天已經領取過囉");
			}
			var prizes =await _repo.GetPrizes();
			List<int> prizesList=new List<int>();
			foreach (var prize in prizes)
			{
				for(int i=0;i<prize.Qty;i++)
				{
					prizesList.Add((int)prize.CouponId);
				}
			}
			Random random = new Random();
			int randomIndex = random.Next(prizesList.Count());
			int resultCouponId = prizesList[randomIndex];
			var resultCouponName =await _repo.AddCouponAndRecordMemberPlay(memberId, resultCouponId);
			return AddCouponResult.Success(resultCouponName);

		}
        public async Task<IEnumerable<DailyGameRateDto>> GetDailyGameRate()
        {
            var prizes = await _repo.GetPrizes();
			var allPrizesCount = prizes.Select(p => p.Qty).Sum();
			List<DailyGameRateDto> result = new List<DailyGameRateDto>();
			foreach(var prize in prizes)
			{
				var coupon =await _repo.FindCoupon((int)prize.CouponId);
				decimal rate = (decimal)((decimal)prize.Qty / allPrizesCount)*100;
				DailyGameRateDto dto = new DailyGameRateDto()
				{
					PrizeName = coupon.CouponName,
					Rate = rate
				};
				result.Add(dto);
            }
			return result;
        }
		public async Task<WeeklyGameResult> PlayWeeklyGame(int[] ansAry, int memberId)
		{
			CouponSetting[] couponSettings = await _repo.GetWeeklyGameCouponSettings();
			var ans1 = ansAry[0];
			var ans2 = ansAry[1];
			var ans3 = ansAry[2];
			var ans4 = ansAry[3];
			var ans5 = ansAry[4];
			var choco = new TestResult(0, couponSettings[0]);
			var strawberry = new TestResult( 0, couponSettings[1]);
			var mocha = new TestResult( 0, couponSettings[2]);
			var taro = new TestResult( 0, couponSettings[3]);
			var beer = new TestResult( 0, couponSettings[4]);
			//巧，莓，抹茶，芋頭，酒
			switch (ans1)
			{
				case 1:
					strawberry.Score += 1;
					beer.Score+=1;
					break;
				case 2:
					beer.Score += 1;
					choco.Score += 1;
					break;
				case 3:
					strawberry.Score += 1;
					taro.Score+= 1;
					break;
				case 4:
					taro.Score += 1;
					mocha.Score += 1;
					break;
				case 5:
					mocha.Score += 1;
					choco.Score += 1;
					break;
				default:
					break;
			}
			switch(ans2)
			{
				case 1:
					strawberry.Score += 1;
					taro.Score += 1;
					break;
				case 2:
					mocha.Score += 1;
					taro.Score += 1;
					break;
				case 3:
					beer.Score += 1;
					choco.Score += 1;
					break;
				case 4:
					choco.Score += 1;
					mocha.Score += 1;
					break;
				case 5:
					strawberry.Score += 1;
					beer.Score += 1;
					break;
                default:
                    break;
            }
			switch (ans3)
			{
				case 1:
					strawberry.Score += 1;
					beer.Score += 1;
					break;
				case 2:
					mocha.Score += 1;
					strawberry.Score += 1;
					break;
				case 3:
					beer.Score += 1;
					choco.Score += 1;
					break;
				case 4:
					taro.Score += 1;
					choco.Score += 1;
					break;
				case 5:
					mocha.Score += 1;
					taro.Score += 1;
					break;
                default:
                    break;
            }
			switch (ans4)
			{
				case 1:
					taro.Score += 1;
					choco.Score += 1;
					break;
				case 2:
					mocha.Score += 1;
					strawberry.Score += 1;
					break;
				case 3:
					beer.Score += 1;
					strawberry.Score += 1;
					break;
				case 4:
					mocha.Score += 1;
					taro.Score += 1;
					break;
				case 5:
					beer.Score += 1;
					choco.Score += 1;
					break;
                default:
                    break;
            }
			switch (ans5)
			{
				case 1:
					strawberry.Score += 1;
					mocha.Score += 1;
					break;
				case 2:
					beer.Score += 1;
					choco.Score += 1;
					break;
				case 3:
					choco.Score += 1;
					strawberry.Score += 1;
					break;
				case 4:
					mocha.Score += 1;
					taro.Score += 1;
					break;
				case 5:
					beer.Score += 1;
					taro.Score += 1;
					break;
                default:
                    break;
            }
			var resultAry = new TestResult[] { choco, strawberry, mocha, beer, taro };
			var orderedResultAry = resultAry.OrderByDescending(r=>r.Score).ToArray();
			var top = orderedResultAry[0];
			var topAry = orderedResultAry.Where(r=>r.Score==top.Score).ToArray();
			Random random = new Random();
			int randomIndex = random.Next(topAry.Length);
			var result = topAry[randomIndex];
            var coupon = await _repo.FindCoupon((int)result.Setting.CouponId);
            AddCouponResult couponResult;
			if (await _repo.HasPlayedWeeklyGame(memberId))
			{
				couponResult = AddCouponResult.Fail("您已經領取過囉~");
			}
			else
			{
				await _repo.AddCouponAndRecordMemberPlayWeeklyGame(memberId, coupon);
				couponResult = AddCouponResult.Success(coupon.CouponName);
            }
			string title,content,image;
			switch (result.Setting.CouponSettingId)
			{
				case 9:
					title = "巧克力";
					content = "巧克力甜點是經典的享受，濃郁的可可味道總是能在每一口中帶來愉悅。無論是朱古力蛋糕、布朗尼，還是朱古力曲奇，巧克力的豐富風味總是能夠滿足你的味蕾，讓你感受到甜蜜的享受。";
					image = "心理測驗-巧克力.jpg";
					break;
				case 10:
                    title = "草莓";
                    content = "草莓甜點散發著天然的水果香氣，帶來清新的口感和微酸的滋味。從草莓奶油蛋糕到草莓塔，這種口味充滿活力，讓人感受到自然的甜美，是一個完美的夏季選擇。";
					image = "心理測驗-草莓.jpg";
					break;
                case 11:
                    title = "抹茶";
                    content = "抹茶甜點代表了日本的精緻和平衡。抹茶口味的冰淇淋、抹茶拿鐵和抹茶蛋糕都帶有微妙的苦甜味道，深受喜愛。這種風味融合了獨特的茶香和奶香，給人一種優雅的感覺。";
					image = "心理測驗-抹茶.jpg";
					break;
                case 12:
                    title = "芋頭";
                    content = "芋頭甜點來自東亞的傳統美食，帶來濃厚的口感和淡淡的甜味。從芋泥包到芋圓湯，芋頭的獨特風味總是令人愉悅。它的豐富和濃郁讓你感受到溫暖和滿足。";
					image = "心理測驗-芋頭.jpg";
					break;
                case 13:
                    title = "微醺";
                    content = "微醺口味的甜點常常融合了微妙的酒香和甜蜜的滋味。從香檳蛋糕到葡萄酒巧克力，這種口味帶來一種恬淡的愉悅感，適合在特別的時刻品味，將微醺的情感與美味融合在一起。";
					image = "心理測驗-微醺.jpg";
					break;
				default:
                    title = "xxxxx";
                    content = "xxxxx";
					image = "xxxxx";
					break;
            }
			var discountGroupId = coupon.DiscountGroupId;
			var dessertsInDiscountGroup = await _repo.FindSuggestProduct((int)discountGroupId);
			var suggestProducts = dessertsInDiscountGroup.Select(d => d.ToSuggestProductDto());
			var weeklyGameResult = new WeeklyGameResult()
			{
				Content = content,
				Title = title,
				Image = image,
				Result = couponResult,
				SuggestProducts = suggestProducts
			};
			return weeklyGameResult;
		}
        public async Task<bool> CancelUsingCoupon(int memberId)
        {
            var result = await _repo.CancelUsingCoupon(memberId);
            return result;
        }

        public async Task<object?> GetUsingCoupon(int memberId)
        {
			var result = await _repo.GetUsingCoupon(memberId);
			return result;
        }
        private async Task<IEnumerable<MemberCoupon>> DoThisToGetCouponMeetCriteria(int memberId)
		{
			var coupons = await _repo.GetUsableCoupon(memberId);
			//先把一定可以用的優惠券加進來
			var couponsMeetCriteria = coupons.Where(mc => mc.Coupon.DiscountGroupId == null && mc.Coupon.LimitType == null).ToList();
			//列出需要檢查的
			var waitToCheck = coupons.Except(couponsMeetCriteria);
			//找出此會員的購物車
			var cart = await _repo.GetDessertCart(memberId);
			var cartItems = cart.DessertCartItems.Select(dci => new DCIwithDiscountPrice()
			{
				UnitPrice = dci.Specification.UnitPrice,
				DiscountPrice = dci.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
												  ? dci.Dessert.Discounts.First().DiscountPrice : null,
				Qty = dci.Quantity
			});
			var totalPrice = cartItems.Select(i => i.DiscountPrice == null ? i.UnitPrice * i.Qty : (Math.Round((decimal)i.UnitPrice * ((decimal)i.DiscountPrice / 100), 0, MidpointRounding.AwayFromZero)) * i.Qty).Sum();

			//列出購物車所優商品的id
			var cartItemsDessertIds = cart.DessertCartItems.Select(dci => dci.DessertId);
			//列出id跟數量
			var dessertIdAndQtys = cart.DessertCartItems.Select(dci => new {
				Id = dci.DessertId,
				Qty = dci.Quantity
			}).ToArray();
			//檢查剩下的優惠券
			foreach (MemberCoupon coupon in waitToCheck)
			{
				if (coupon.Coupon.DiscountGroupId == null)
				{
					if (totalPrice >= coupon.Coupon.LimitValue)
					{
						couponsMeetCriteria.Add(coupon);
					}
					else continue;
				}
				else
				{
					var discountGroupDessertIds = coupon.Coupon.DiscountGroup.DiscountGroupItems.Select(dgi => dgi.DessertId);

					if (coupon.Coupon.LimitType == null)
					{
						var commonItems = cartItemsDessertIds.Intersect(discountGroupDessertIds);
						if (commonItems.Any())
						{
							couponsMeetCriteria.Add(coupon);
						}
						else continue;
						//這種方法執行效率較佳但上面的比較簡潔
						//for (int i = 0; i < discountGroupDessertIds.Length; i++)
						//{
						//    if (cartItemsDessertIds.Contains(discountGroupDessertIds[i]))
						//    {
						//        couponsMeetCriteria.Add(coupon);
						//        break;
						//    }
						//    else continue;
						//}
					}
					else
					{
						var neededCount = coupon.Coupon.LimitValue;
						var buyCount = 0;
						for (int i = 0; i < dessertIdAndQtys.Length; i++)
						{
							if (buyCount >= neededCount)
							{
								couponsMeetCriteria.Add(coupon);
								break;
							}
							else
							{
								if (discountGroupDessertIds.Contains(dessertIdAndQtys[i].Id))
								{
									buyCount += dessertIdAndQtys[i].Qty;
								}
								else continue;
							}
						}
					}
				}
			}
			return couponsMeetCriteria;
		}

        public async Task<object?> DidMemberPlayedGame(int memberId)
        {
            var dailyGame = await _repo.HasPlayedGame(memberId);
			var weeklyGame =await _repo.HasPlayedWeeklyGame(memberId);
			return new {DailyGame= dailyGame, WeeklyGame= weeklyGame};
        }
    }
}

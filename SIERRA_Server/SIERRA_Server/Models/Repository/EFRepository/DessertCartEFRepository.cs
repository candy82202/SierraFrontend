using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using SIERRA_Server.Models.DTOs.Carts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository
{
    public class DessertCartEFRepository : IDessertCartRepository

    {
        private readonly AppDbContext _context;
        public DessertCartEFRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateCartItem(string username, int dessertId, int specificationId, int quantity)
        {
            var cart = await GetOrCreateCart(username); 
            var existingItem = cart.DessertCartItems.FirstOrDefault(item => item.DessertId == dessertId && item.SpecificationId == specificationId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _context.DessertCartItems.Find(existingItem.Id).Quantity = existingItem.Quantity;

            }
            else
            {
                var newItem = new DessertCartItem
                {
                    DessertCartId = cart.Id,
                    DessertId = dessertId,
                    SpecificationId = specificationId,
                    Quantity = quantity
                };
                _context.DessertCartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateCartItemQty(int id, int quantity)
        {

            var cartItem = _context.DessertCartItems.FirstOrDefault(item => item.Id == id);
            var cart = _context.DessertCarts.Find(cartItem.DessertCartId);


            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                if (cartItem.Quantity < 1)
                {
                    // 如果数量小于1，将项目从购物车中删除
                    _context.DessertCartItems.Remove(cartItem);
                }
                else
                {
                    _context.DessertCartItems.Update(cartItem);
                }
            }
            cart.MemberCouponId = null;
            cart.DiscountPrice = null;
            await _context.SaveChangesAsync();
        }
        public async Task<CartDTO> GetOrCreateCart(string username)
        {
            var cart = await _context.DessertCarts
             .Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Dessert).ThenInclude(d => d.Discounts)
             .Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Dessert).ThenInclude(d => d.DessertImages)
             .Include(dc => dc.DessertCartItems).ThenInclude(dci => dci.Specification)
             .FirstOrDefaultAsync(dc => dc.Username == username);

            if (cart == null)
            {
                cart = new DessertCart
                {
                    Username = username,
                    MemberCouponId = null
                };
                _context.DessertCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartDTO = new CartDTO
            {
                Id = cart.Id,
                Username = cart.Username,
                DessertCartItems = cart.DessertCartItems.Select(dci => new CartItemDTO
                {
                    Id = dci.Id,
                    SpecificationId = dci.SpecificationId,
                    DessertCartId = dci.DessertCartId,
                    DessertId = dci.DessertId,
                    Quantity = dci.Quantity,
                    Flavor = dci.Specification.Flavor,
                    Size = dci.Specification.Size,
                    DessertImageName = dci.Dessert.DessertImages.FirstOrDefault()?.DessertImageName,
                    DessertName = dci.Dessert.DessertName,
                    //dci.Dessert.Discounts.First().DiscountPrice 這是折扣的折扣率 / 100 百分比切換為小數
                    UnitPrice = (int)(dci.Dessert.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
                    ? Math.Round((decimal)dci.Specification.UnitPrice * ((decimal)dci.Dessert.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero) : dci.Specification.UnitPrice)
                }).ToList()
            };

            return cartDTO;
        }
        public async Task<int> GetCartTotalPrice(string username)
        {
            var cartDto = await GetOrCreateCart(username);
            return cartDto.DessertCartItems.Sum(item => item.SubTotal);
        }

        public async Task<string> GetUsernameById(int? memberId)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == memberId);

            if (member != null)
            {
                return member.Username;
            }
            else
            {
                return null; // 或者返回一个默认值
            }
        }
    }
}

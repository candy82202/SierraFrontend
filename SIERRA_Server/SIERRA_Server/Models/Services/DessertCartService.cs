using SIERRA_Server.Models.DTOs.Carts;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Services
{
    public class DessertCartService
    {
        private IDessertCartRepository _repo;
        public DessertCartService(IDessertCartRepository repo)
        {
            _repo=repo;
        }
        public async Task<CartDTO> GetOrCreateCart(string username)
        {
            var cartDto = await _repo.GetOrCreateCart(username);
            return cartDto;
        }
        public async Task AddOrUpdateCartItem(string username, int dessertId, int specificationId, int quantity)
        {
            await _repo.AddOrUpdateCartItem(username, dessertId, specificationId, quantity);
        }
        public async Task UpdateCartItemQty(int id, int quantity)
        { 
            await _repo.UpdateCartItemQty(id, quantity);
        }
        public async Task<int> GetCartTotalPrice(string username) 
        {
            var cartPrice = await _repo.GetCartTotalPrice(username);
            return cartPrice;
        }

    }
}

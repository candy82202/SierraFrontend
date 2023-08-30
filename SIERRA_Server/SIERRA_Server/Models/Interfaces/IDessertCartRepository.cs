using SIERRA_Server.Models.DTOs.Carts;

namespace SIERRA_Server.Models.Interfaces
{
    public interface IDessertCartRepository
    {
        Task<CartDTO> GetOrCreateCart(string username);
        Task AddOrUpdateCartItem(string username, int dessertId, int specificationId, int quantity);
        Task UpdateCartItemQty(int id, int quantity);
        Task<int> GetCartTotalPrice(string username);
        Task<string> GetUsernameById(int? memberId);
    }
}

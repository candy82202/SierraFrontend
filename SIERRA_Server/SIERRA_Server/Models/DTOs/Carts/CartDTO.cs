using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Carts
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public  IEnumerable<CartItemDTO> DessertCartItems { get; set; }
        public int Total
        {
            get 
            {
                return DessertCartItems.Sum(item => item.SubTotal);
            }
        }
        public bool AllowCheckout => DessertCartItems.Any();
    }
}

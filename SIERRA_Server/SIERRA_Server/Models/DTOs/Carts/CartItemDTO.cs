using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Carts
{
    public class CartItemDTO
    {
        public string Username { get; set; }
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        public int DessertCartId { get; set; }
        public int DessertId { get; set; }
        public int Quantity { get; set; }

        public Dessert Dessert { get; set; }
        public Specification Specification { get; set; }
        public int SubTotal => Specification.UnitPrice * Quantity;
    }
}

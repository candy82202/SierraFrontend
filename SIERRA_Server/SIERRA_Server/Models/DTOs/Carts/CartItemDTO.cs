using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Carts
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        public int DessertCartId { get; set; }
        public int DessertId { get; set; }
        public int Quantity { get; set; }
        public string DessertImageName { get; set; }
        public int UnitPrice { get; set; }
        public string DessertName { get; set; }
        public int SubTotal => UnitPrice * Quantity;
    }
}

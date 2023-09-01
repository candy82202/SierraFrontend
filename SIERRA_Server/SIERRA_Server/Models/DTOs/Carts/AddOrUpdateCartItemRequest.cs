namespace SIERRA_Server.Models.DTOs.Carts
{
    public class AddOrUpdateCartItemRequest
    {
        public string Username { get; set; }
        public int DessertId { get; set; }
        public int SpecificationId { get; set; }
        public int Quantity { get; set; }
    }
}

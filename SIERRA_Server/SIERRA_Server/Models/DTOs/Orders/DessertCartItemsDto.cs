namespace SIERRA_Server.Models.DTOs.Orders
{
    public class DessertCartItemsDto
    {
        public string DessertName { get; set; }
        public string DessertImage { get; set; }
        public int DessertCartItemId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string Flavor { get; set; }
        public string Size { get; set; }
    }
}
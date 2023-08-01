using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs
{
    public class DessertListDTO
    {
        public Dessert Dessert { get; set; }
        public int DessertId { get; set; }
        public string DessertName { get; set; }
        public int UnitPrice { get; set; }
        public string DessertImageName { get; set; }
    }
}

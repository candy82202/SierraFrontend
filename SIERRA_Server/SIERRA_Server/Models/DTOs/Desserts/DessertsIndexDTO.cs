using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Desserts
{
    public class DessertsIndexDTO
    {
        public int DessertId { get; set; }
        public string DessertName { get; set; }
        public int UnitPrice { get; set; }
        public string DessertImageName { get; set; }
        public Specification Specification { get; set; }
        public string Flavor { get; set; }
        public string Size { get; set; }
    }
}

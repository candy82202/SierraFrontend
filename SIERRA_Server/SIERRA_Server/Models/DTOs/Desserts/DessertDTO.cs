using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Desserts
{
    public class DessertDTO
    {
        public int DessertId { get; set; }

        public string DessertName { get; set; }

        public string CategoryName { get; set; }

        public int UnitPrice { get; set; }
        public decimal DiscountedPrice { get; }
        public decimal DessertDiscountPrice { get; }  // Define as private set
        public DessertDTO(decimal dessertDiscountPrice, int unitPrice)
        {
            DessertDiscountPrice = dessertDiscountPrice;
            DiscountedPrice = CalculateDiscountedPrice(unitPrice, dessertDiscountPrice);
        }

        private decimal CalculateDiscountedPrice(int unitPrice, decimal dessertDiscountPrice)
        {
            return dessertDiscountPrice != 0
                ? Math.Round(unitPrice * (dessertDiscountPrice / 100), 0, MidpointRounding.AwayFromZero)
                : UnitPrice;
        }
        public string Description { get; set; }

        public List<DessertImage> DessertImages { get; set; } // Change this line
        public List<SpecificationDTO> Specifications { get; set; } // List of SpecificationDTO objects
     
    }
    public class SpecificationDTO
    {
        public int SpecificationId { get; set; }
        public string Size { get; set; }
        public string Flavor { get; set; }
        public int? UnitPrice { get; set; }
        // Include other properties here
    }
}

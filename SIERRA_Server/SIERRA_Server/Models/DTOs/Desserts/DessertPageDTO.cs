using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Desserts
{
      public class DessertPageDTO
    {
        public int DessertId { get; set; }
        public string DessertName { get; set; }
       
        public int UnitPrice { get; set; }
        public decimal DiscountedPrice => CalculateDiscountedPrice(UnitPrice, DessertDiscountPrice);
        public decimal DessertDiscountPrice { get; set; }
        public string ImageName { get; set; }
        public int SpecificationId { get; set; }
        public int TotalPages { get; set; }

        private decimal CalculateDiscountedPrice(int unitPrice, decimal dessertDiscountPrice)
        {
            return dessertDiscountPrice != 0
                ? Math.Round(unitPrice * (dessertDiscountPrice / 100), 0, MidpointRounding.AwayFromZero)
                : unitPrice;
        }
    }
}

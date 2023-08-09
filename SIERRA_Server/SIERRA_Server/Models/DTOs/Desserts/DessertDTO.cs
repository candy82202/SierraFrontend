using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Desserts
{
    public class DessertDTO
    {
        public int DessertId { get; set; }

        public string DessertName { get; set; }

        public string CategoryName { get; set; }

        //public int? UnitPrice { get; set; }

        public string Description { get; set; }

        public string DessertImageName { get; set; }
        //public Specification Specification { get; set; }
        //public string Flavor { get; set; }
        //public string Size { get; set; }
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

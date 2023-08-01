namespace SIERRA_Server.Models.DTOs
{
    public class DessertDTO
    {
        public int DessertId { get; set; }

        public string DessertName { get; set; }

        public string CategoryName { get; set; }

        public int? UnitPrice { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}

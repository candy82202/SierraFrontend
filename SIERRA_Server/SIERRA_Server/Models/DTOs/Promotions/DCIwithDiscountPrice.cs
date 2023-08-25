using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SIERRA_Server.Models.DTOs.Promotions
{
    public class DCIwithDiscountPrice
    {
        public int UnitPrice { get; set; }
        public int? DiscountPrice { get; set; }
        public int Qty { get; set; }
    }
}
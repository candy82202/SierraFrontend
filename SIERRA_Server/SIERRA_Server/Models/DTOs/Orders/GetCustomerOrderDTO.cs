using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Orders
{
    public class GetCustomerOrderDTO
    {
        public int? Id { get; set; }
        //public int DessertOrderStatusId { get; set; }
        public string Username { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? Recipient { get; set; }
        public string? RecipientPhone { get; set; }
        public string? RecipientAddress { get; set; }
        public int? ShippingFee { get; set; }
        public int? DessertOrderTotal { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? Note { get; set; }
        public string? PayMethod { get; set; }
        public string? CouponName { get; set; }
        public string? DiscountInfo { get; set; }
        //public int OrderStatusId { get; set; }
        public string? StatusName { get; set; }
        public List<ItemDto>? DessertOrderDetails { get; set; }

    }
    public class ItemDto
    {
        // 甜點訂單明細
        //public int Id { get; set; }
        //public int SpecificationId { get; set; }
        //public int DessertOrderId { get; set; }
        //public int DessertId { get; set; }
        public string DessertName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int Subtotal { get; set; }

    }
}

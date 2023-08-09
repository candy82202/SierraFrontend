namespace SIERRA_Server.Models.DTOs.Orders
{
    public class CreateDessertOrderDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int DessertOrderStatusId { get; set; }
        public int? MemberCouponId { get; set; }
        public DateTime CreateTime { get; set; }
        public string Recipient { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public int ShippingFee { get; set; }
        public int DessertOrderTotal { get; set; }
        public string DeliveryMethod { get; set; }
        public string Note { get; set; }
        public string DiscountInfo { get; set; }
    }
}

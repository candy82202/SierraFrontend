namespace SIERRA_Server.Models.DTOs.Orders
{
    public class CancelOrderDto
    {
        public int LessonOrderId { get; set; }
        public string OrderCancellationReason { get; set; }
    }
}

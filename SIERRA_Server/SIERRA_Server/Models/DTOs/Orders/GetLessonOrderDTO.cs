namespace SIERRA_Server.Models.DTOs.Orders
{
    public class GetLessonOrderDTO
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string? StatusName { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? LessonOrderTotal { get; set; }
        public string? PayMethod { get; set; }
        public string? Note { get; set; }
        public string? OrderCancellationReason { get; set; }
        public List<LessonItemDto>? LessonOrderDetails { get; set; }
    }

    public class LessonItemDto
    {
        //課程訂單明細
        public string LessonTitle { get; set; }
        public int NumberOfPeople { get; set; }
        public int LessonUnitPrice { get; set; }
        public int Subtotal { get; set; }
        public DateTime LessonTime { get; set; }
    }
}

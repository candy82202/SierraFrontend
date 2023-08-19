namespace SIERRA_Server.Models.DTOs.Orders
{
    public class CreateLessonOrderDTO
    {
        public int? Id { get; set; }
        public int? MemberId { get; set; }
        public string Username { get; set; }
        public int? LessonOrderStatusId { get; set; }
        public DateTime CreateTime { get; set; }
        public int LessonOrderTotal { get; set; }
        public string PayMethod { get; set; }
        public string? Note { get; set; }
        public string LessonTitle { get; set; } 
        public int ActualCapacity { get; set; }

        public int? LessonOrderId { get; set; }
        public int? LessonId { get; set; }
       
        public int NumberOfPeople { get; set; }
        public int LessonUnitPrice { get; set; }
        public int Subtotal { get; set; }
        //public DateTime LessonTime { get; set; }


        //public string OrderCancellationReason { get; set; }
    }
}

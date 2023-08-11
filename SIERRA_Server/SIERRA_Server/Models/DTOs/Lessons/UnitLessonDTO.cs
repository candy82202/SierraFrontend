using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Lessons {
    public class UnitLessonDTO {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public int LessonPrice { get; set; }
        public string LessonDessert { get; set; }

        public string LessonInfo { get; set; }

        public string LessonDetail { get; set; }

        public DateTime LessonTime { get; set; }
        public int LessonHours { get; set; }
        public int MaximumCapacity { get; set; }
        public bool LessonStatus { get; set; }
        public int ActualCapacity { get; set; }
        public DateTime LessonEndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual IEnumerable<LessonImage> LessonImages { get; set; }
    }
}

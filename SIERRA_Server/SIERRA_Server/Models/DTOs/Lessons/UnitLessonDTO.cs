using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Lessons {
    public class UnitLessonDTO {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public int LessonPrice { get; set; }
        public string LessonDessert { get; set; }

        public string LessonInfo { get; set; }

        public string LessonDetail { get; set; }

        public virtual IEnumerable<LessonImage> LessonImages { get; set; }
    }
}

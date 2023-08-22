using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Lessons {
    public class LessonDTO {
        public List<Lesson>? Lessons { get; set; }
    }

    public class LessonDto
    {
        public bool IsLessonExpired { get; set; }
        public int LessonId { get; set; }
        public int LessonCategoryId { get; set; }
        public int TeacherId { get; set; }
        public string LessonTitle { get; set; }
        public string LessonInfo { get; set; }
        public string LessonDetail { get; set; }
        public string LessonDessert { get; set; }
        public DateTime LessonTime { get; set; }
        public int LessonHours { get; set; }
        public int MaximumCapacity { get; set; }
        public int LessonPrice { get; set; }
        public bool LessonStatus { get; set; }
        public int ActualCapacity { get; set; }
        public DateTime LessonEndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public List<string> LessonImageName  { get; set; }
        public string LessonCategoryName { get; set; }
        public string TeacherName { get; set; }
        public virtual LessonCategory LessonCategory { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}

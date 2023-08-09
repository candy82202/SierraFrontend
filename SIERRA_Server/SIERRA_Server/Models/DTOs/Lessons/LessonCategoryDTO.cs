using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Lessons {
    public class LessonCategoryDTO {

        public List<LessonCategoryDTOItem>? Categories { get; set; }
    }
    public class LessonCategoryDTOItem {

        public int LessonCategoryId { get; set; }

        public string? LessonCategoryName { get; set; }
    }
}

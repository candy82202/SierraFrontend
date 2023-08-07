using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Lessons {
    public class TeacherDTO {
        public List<Teacher>? Teachers { get; set; }
        public int TotalPages { get; set; }
    }
}

using SIERRA_Server.Models.DTOs.Lessons;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Services {
    public class LessonService {

        private ILessonRepository _repo;

        public LessonService(ILessonRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<LessonCategoryDtoItem>> GetLessonCategoriesAsync()
        {
            var lessoncategory = await _repo.GetLessonCategoriesAsync();
            return lessoncategory;
        }

        public async Task<List<LessonDto>> GetLessonsAsync(string? categoryName)
        {
           

            var lesson = await _repo.GetLessonsAsync(categoryName);
          
            return lesson;
        }
    }
}

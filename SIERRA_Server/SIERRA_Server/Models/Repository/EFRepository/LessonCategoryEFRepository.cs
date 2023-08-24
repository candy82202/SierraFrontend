using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Lessons;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.EFRepository {
    public class LessonCategoryEFRepository : ILessonRepository {

        private readonly AppDbContext _appDbContext;
        public LessonCategoryEFRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<LessonCategoryDtoItem>> GetLessonCategoriesAsync()
        {
            var dom = await _appDbContext.LessonCategories
                                .Include(l => l.Lessons)
                                .Select(entity =>entity.ToLessonCategoryDtoItem())
                                .ToListAsync();
            return dom;           
        }

        public async Task<List<LessonDto>> GetLessonsAsync(string? categoryName)
        {

            IQueryable<Lesson> lessons = _appDbContext.Lessons
                             .Include(lt => lt.Teacher)
                             .Include(lm => lm.LessonImages)
                             .Include(lc => lc.LessonCategory)
                             .Where(l => l.Teacher.TeacherStatus == true && l.LessonStatus == true);

                 if (!string.IsNullOrEmpty(categoryName))
            {
                lessons = lessons.Where(lc =>lc.LessonCategory.LessonCategoryName.Contains(categoryName));
            };

            //獲取當前時間
            DateTime currentTime = DateTime.Now;

            var result = await lessons
                                .Select(entity => entity.ToLessonDto(currentTime))
                                .ToListAsync();
            
               
            return result;

        }
    }
}

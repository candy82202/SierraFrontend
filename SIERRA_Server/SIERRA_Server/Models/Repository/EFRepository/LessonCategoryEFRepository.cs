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
    }
}

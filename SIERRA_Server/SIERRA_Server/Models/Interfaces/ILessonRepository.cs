using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.DTOs.Lessons;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.Interfaces {
    public interface ILessonRepository {

        Task<List<LessonCategoryDtoItem>> GetLessonCategoriesAsync();
    }
}

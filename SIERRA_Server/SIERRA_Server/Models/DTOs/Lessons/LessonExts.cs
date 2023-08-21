using Microsoft.CodeAnalysis.CSharp.Syntax;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Lessons {
    public static class LessonExts {

        public static LessonCategoryDtoItem ToLessonCategoryDtoItem(this LessonCategory entity)
        {
            return new LessonCategoryDtoItem
            {
                LessonCategoryId = entity.LessonCategoryId,
                LessonCategoryName = entity.LessonCategoryName,
            };
        }

    }
}

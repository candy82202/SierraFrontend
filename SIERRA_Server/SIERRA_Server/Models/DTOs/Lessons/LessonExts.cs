using Microsoft.CodeAnalysis.CSharp.Syntax;
using SIERRA_Server.Models.EFModels;
using System.Diagnostics.Eventing.Reader;

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

        public static LessonDto ToLessonDto(this Lesson entity, DateTime currentTime)
        {
            bool isLessonExpired = entity.LessonTime <= currentTime;
            bool lessonStatus = entity.LessonStatus && !isLessonExpired;

            return new LessonDto
            {
                IsLessonExpired = isLessonExpired,
                LessonId = entity.LessonId,
                LessonCategoryId = entity.LessonCategoryId,
                TeacherId = entity.TeacherId,
                LessonTitle = entity.LessonTitle,
                LessonInfo = entity.LessonInfo,
                LessonDessert = entity.LessonDessert,
                LessonTime = entity.LessonTime,
                MaximumCapacity = entity.MaximumCapacity,
                ActualCapacity = entity.ActualCapacity,
                LessonPrice = entity.LessonPrice,
                LessonStatus = lessonStatus,
                LessonImageName = entity.LessonImages.Select(lm => lm.LessonImageName).ToList(),
                LessonCategoryName = entity.LessonCategory.LessonCategoryName,
                TeacherName = entity.Teacher.TeacherName,
            };
        }

    }
}

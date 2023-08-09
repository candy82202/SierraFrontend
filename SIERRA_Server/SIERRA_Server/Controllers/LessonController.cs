using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Lessons;
using SIERRA_Server.Models.EFModels;
using System.Diagnostics;

namespace SIERRA_Server.Controllers {

    public class LessonController : Controller {

        private readonly AppDbContext _context;
        public LessonController(AppDbContext context)
        {
            _context = context;
        }
        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<LessonDTO>> GetLessons(string? categoryName)
        {
            if (_context == null)
            {
                return NotFound();
            }

            var lessons =  _context.Lessons.Include(t => t.Teacher)
                                                              .Include(lm => lm.LessonImages)
                                                              .Include(lc => lc.LessonCategory)
                                                              .Where(l => l.Teacher.TeacherStatus == true && l.LessonStatus == true)
                                                              .AsQueryable();
          

            if (!string.IsNullOrEmpty(categoryName))
            {
                lessons = lessons.Include(l => l.LessonCategory)
                                            .Where(lc => lc.LessonCategory.LessonCategoryName.Contains(categoryName));
            }

            LessonDTO lessondto = new LessonDTO();
            lessondto.Lessons = await lessons.ToListAsync();

            return lessondto;
        }

        // GET:category
      
        [HttpGet("category")]
        public async Task<ActionResult<LessonCategoryDTO>> GetLessonCategories()
        {
            if (_context == null)
            {
                return NotFound();
            }

            var lessonCategories = _context.LessonCategories.Include(l => l.Lessons);
                                                                               

            var lessonCategoryDTO = new LessonCategoryDTO();
            lessonCategoryDTO.Categories = await lessonCategories.Select(lc => new LessonCategoryDTOItem
                                                                                                                                   {
                                                                                                                                        LessonCategoryId = lc.LessonCategoryId,
                                                                                                                                        LessonCategoryName = lc.LessonCategoryName
                                                                                                                                    }).ToListAsync();

            return lessonCategoryDTO;
        }

        [HttpGet("lessonId")]
        public async Task<ActionResult<UnitLessonDTO>> GetLessonById(int lessonId)
        {
            if (_context == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.Include(t => t.Teacher)
                                                                    .Include(lm => lm.LessonImages)
                                                                    .Include(lc => lc.LessonCategory)
                                                                    .FirstOrDefaultAsync(l =>l.LessonId == lessonId);

            if(lesson == null)
            {
                return NotFound();
            }

            var lessonDTO = new UnitLessonDTO
            {
                LessonId = lesson.LessonId,
                LessonDessert = lesson.LessonDessert,
                LessonImages = lesson.LessonImages,
                LessonTitle = lesson.LessonTitle,
                LessonDetail = lesson.LessonDetail,
                LessonInfo = lesson.LessonInfo,
                LessonPrice = lesson.LessonPrice,
            };

            return lessonDTO;
        }

    }
}

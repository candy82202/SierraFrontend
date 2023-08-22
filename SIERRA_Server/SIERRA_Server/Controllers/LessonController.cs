using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Lessons;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Services;
using System.Diagnostics;


namespace SIERRA_Server.Controllers {


    namespace SIERRA_Server.Controllers {

        
        [Route("api/[controller]")]
        [ApiController]
        public class LessonController : ControllerBase {

            private readonly AppDbContext _context;
            private readonly ILessonRepository _repo;
            public LessonController(AppDbContext context, ILessonRepository repo)
            {
                _context = context;
                _repo = repo;
            }

            //[HttpGet]
            //public async Task<ActionResult<LessonDTO>> GetLessons(string? categoryName)
            //{
            //    if (_context == null)
            //    {
            //        return NotFound();
            //    }

            //    var lessons = _context.Lessons.Include(t => t.Teacher)
            //                                                      .Include(lm => lm.LessonImages)
            //                                                      .Include(lc => lc.LessonCategory)
            //                                                      .Where(l => l.Teacher.TeacherStatus == true && l.LessonStatus == true)
            //                                                      .AsQueryable();


            //    if (!string.IsNullOrEmpty(categoryName))
            //    {
            //        lessons = lessons.Include(l => l.LessonCategory)
            //                                    .Where(lc => lc.LessonCategory.LessonCategoryName.Contains(categoryName));
            //    }

            //    LessonDTO lessondto = new LessonDTO();
            //    lessondto.Lessons = await lessons.ToListAsync();

            //    return lessondto;
            //}

            // GET:category

            //[HttpGet("category")]
            //public async Task<ActionResult<LessonCategoryDTO>> GetLessonCategories()
            //{
            //    if (_context == null)
            //    {
            //        return NotFound();
            //    }

            //    var lessonCategories = _context.LessonCategories.Include(l => l.Lessons);


            //    var lessonCategoryDTO = new LessonCategoryDTO();
            //    lessonCategoryDTO.Categories = await lessonCategories.Select(lc => new LessonCategoryDTOItem
            //    {
            //        LessonCategoryId = lc.LessonCategoryId,
            //        LessonCategoryName = lc.LessonCategoryName
            //    }).ToListAsync();
                
            //    return lessonCategoryDTO;
            //}

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
                                                                        .FirstOrDefaultAsync(l => l.LessonId == lessonId);

                if (lesson == null)
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
                    LessonEndTime = lesson.LessonEndTime,
                    LessonHours = lesson.LessonHours,
                    LessonStatus = lesson.LessonStatus,
                    LessonTime  = lesson.LessonTime,
                    ActualCapacity  = lesson.ActualCapacity,
                    CreateTime = lesson.CreateTime,
                    MaximumCapacity = lesson.MaximumCapacity,
                };

                return lessonDTO;
            }

            [HttpGet("category")]
            public async Task<IActionResult> GetLessonCategoriesAsync()
            {
                var service = new LessonService(_repo);
                var lessonCategory = await service.GetLessonCategoriesAsync();
                return Ok(lessonCategory);
            }

            [HttpGet("lesson")]
            public async Task<IActionResult> GetLessonsAsync(string? categoryName)
            {
                var service = new LessonService(_repo); 
                var lesson = await service.GetLessonsAsync(categoryName);
                return Ok(lesson);
            }

        }
    
    
    
    }

}


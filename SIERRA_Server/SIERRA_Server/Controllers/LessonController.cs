using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Lessons;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Controllers {
    public class LessonController : Controller {

        private readonly AppDbContext _context;
        public LessonController(AppDbContext context)
        {
            _context = context;
        }
        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<LessonDTO>> GetLessons(string? keyword)
        {
            if(_context == null)
            {
                return NotFound();
            }

            var lessons =  _context.Lessons.Include(t => t.Teacher)
                                                              .Include(lm => lm.LessonImages)
                                                              .Where(l => l.Teacher.TeacherStatus == true)
                                                              .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                lessons = lessons.Include(l => l.LessonCategory)
                                            .Where(lc => lc.LessonCategory.LessonCategoryName.Contains(keyword));
            }

            LessonDTO lessondto = new LessonDTO();
            lessondto.Lessons = await lessons.ToListAsync();

            return lessondto;
        }
    }
}

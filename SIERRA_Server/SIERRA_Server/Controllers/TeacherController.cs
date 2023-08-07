using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Lessons;
using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Controllers {
    public class TeacherController : Controller {

        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<TeacherDTO>> GetTeachers(int page = 1, int pageSize = 3)
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }
            var teachers = _context.Teachers.Include(l => l.Lessons)
                                                                .Where(t => t.TeacherStatus == true)
                                                                .AsQueryable();

            int totalCount = teachers.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            teachers = teachers.Skip(pageSize * (page - 1)).Take(pageSize);

            TeacherDTO teacherDTO = new TeacherDTO();
            teacherDTO.Teachers = await teachers.ToListAsync();
            teacherDTO.TotalPages = totalPages;

            return teacherDTO;
        }
    }
}

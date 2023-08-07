using Microsoft.AspNetCore.Mvc;

namespace SIERRA_Server.Controllers {
    public class LessonController : Controller {
        public IActionResult Index()
        {
            return View();
        }
    }
}

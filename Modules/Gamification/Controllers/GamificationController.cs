using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Modules.Gamification.Controllers
{
    public class GamificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

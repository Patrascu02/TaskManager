using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Modules.Tasks.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    



    // GET: Tasks/Priorities
        public IActionResult Priorities()
        {
            // Returnăm o listă simplă, statică, pentru informare
            return View();
        }
    }
}

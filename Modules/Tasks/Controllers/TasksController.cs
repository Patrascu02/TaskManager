using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Modules.Tasks.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

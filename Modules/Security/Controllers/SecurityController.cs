using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Modules.Security.Controllers
{
    public class SecurityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

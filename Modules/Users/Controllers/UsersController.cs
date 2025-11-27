using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Modules.Users.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

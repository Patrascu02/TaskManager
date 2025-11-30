using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskManager.Modules.Users.Models;
using TaskManager.Modules.Security.Models;

namespace TaskManager.Modules.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
                              UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Daca utilizatorul NU este logat → pagina publica
            if (!User.Identity.IsAuthenticated)
                return View();

            // Daca este logat → aflam cine este
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return View();

            // Redirect in functie de rol
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("AdminDashboard");

            if (await _userManager.IsInRoleAsync(user, "Manager"))
                return RedirectToAction("ManagerDashboard");

            return RedirectToAction("UserDashboard");
        }

        // ==== DASHBOARD-URI ====

        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult ManagerDashboard()
        {
            return View();
        }

        public IActionResult UserDashboard()
        {
            return View();
        }

        // ==== ALTE METODE EXISTENTE ====

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

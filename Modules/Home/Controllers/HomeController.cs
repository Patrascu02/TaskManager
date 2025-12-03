using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Necesar pentru CountAsync
using TaskManager.Data; // Necesar pentru ApplicationDbContext
using TaskManager.Modules.Users.Models;
using TaskManager.Modules.Security.Models;

namespace TaskManager.Modules.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db; // Adăugat pentru acces la baza de date

        public HomeController(ILogger<HomeController> logger,
                              UserManager<ApplicationUser> userManager,
                              ApplicationDbContext db) // Injectăm DB
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return View();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("AdminDashboard");

            if (await _userManager.IsInRoleAsync(user, "Manager"))
                return RedirectToAction("ManagerDashboard");

            return RedirectToAction("UserDashboard");
        }

        // ==== DASHBOARD-URI ====

        public async Task<IActionResult> AdminDashboard()
        {
            // --- LOGICA MUTATĂ DIN SECURITY CONTROLLER ---

            // 1. Colectăm statistici reale
            var totalUsers = await _userManager.Users.CountAsync();
            var totalTasks = await _db.UserTasks.CountAsync();
            var activeTasks = await _db.UserTasks.CountAsync(t => t.IsActive);

            // 2. Trimitem datele către View
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalTasks = totalTasks;
            ViewBag.ActiveTasks = activeTasks;
            ViewBag.ServerTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            ViewBag.OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            ViewBag.Framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

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
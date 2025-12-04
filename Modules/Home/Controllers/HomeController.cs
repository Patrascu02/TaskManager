using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Necesar pentru CountAsync
using TaskManager.Data; // Necesar pentru ApplicationDbContext
using TaskManager.Modules.Users.Models;
using TaskManager.Modules.Security.Models;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db; // Adaugat pentru acces la baza de date

        public HomeController(ILogger<HomeController> logger,
                              UserManager<ApplicationUser> userManager,
                              ApplicationDbContext db) // Injectam DB
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
                return RedirectToAction("Leaderboard", "Gamification");

            return RedirectToAction("UserDashboard");
        }

        // ==== DASHBOARD-URI ====

        public async Task<IActionResult> AdminDashboard()
        {

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

        public async Task<IActionResult> UserDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index");

            // 1. Luăm task-urile asignate acestui user care NU sunt finalizate încă
            // Status 0 = Assigned, 1 = In Progress, 2 = Completed (Presupunem 2 e finalizat)
            var myTasks = await _db.TaskAssignments
                                   .Include(ta => ta.Task) // Încărcăm detaliile task-ului
                                   .Where(ta => ta.UserId == user.Id && ta.Status != 2)
                                   .OrderBy(ta => ta.Task.DueDate)
                                   .ToListAsync();

            // 2. Putem trimite și date despre XP
            ViewBag.CurrentXp = user.TotalXp;
            ViewBag.CurrentLevel = user.LevelId ?? 1;

            return View(myTasks);
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
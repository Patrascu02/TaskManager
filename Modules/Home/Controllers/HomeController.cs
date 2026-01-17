using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TaskManager.Data;
using TaskManager.Modules.Users.Models;
using TaskManager.Modules.Security.Models;
using TaskManager.Modules.Tasks.Models;
using TaskManager.Modules.Home.Models; // NECESAR pentru AdminDashboardViewModel

namespace TaskManager.Modules.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,
                              UserManager<ApplicationUser> userManager,
                              ApplicationDbContext db)
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

            // Redirecționare în funcție de rol
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("AdminDashboard");

            if (await _userManager.IsInRoleAsync(user, "Manager"))
                return RedirectToAction("Index", "Tasks");

            // Default: User Dashboard
            return RedirectToAction("UserDashboard");
        }

        // ==============================================================
        // 1. ADMIN DASHBOARD (Design Modern "Green Aurora")
        // ==============================================================
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AdminDashboard()
        {
            // A. Colectăm datele reale din Baza de Date
            var totalUsers = await _db.Users.CountAsync();
            var totalTasks = await _db.UserTasks.CountAsync();

            // Task-uri active = cele care sunt "În lucru" (Status 1)
            var activeTasks = await _db.TaskAssignments.CountAsync(ta => ta.Status == 1);

            // CORECTAT: Folosim (int?) pentru a prinde rezultatul SumAsync si ?? 0 pentru a returna un int valid
            var totalXpNullable = await _db.UserXpHistories.SumAsync(x => x.ChangeAmount);
            int totalXp = totalXpNullable ?? 0;

            // B. Luăm ultimele 8 intrări din istoricul de XP ca "Log-uri Recente"
            var recentActivity = await _db.UserXpHistories
                                          .Include(x => x.User) // Încărcăm numele userului
                                          .OrderByDescending(x => x.CreatedAt)
                                          .Take(8)
                                          .ToListAsync();

            // C. Construim Modelul pentru View
            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalTasks = totalTasks,
                ActiveTasks = activeTasks,
                TotalXpAwarded = totalXp,
                RecentLogs = recentActivity,

                // D. Date simulate pentru monitorizare Server
                CpuUsagePercent = new Random().Next(20, 65),
                RamUsageMb = 450,
                RamTotalMb = 1024,
                ActiveSessions = new Random().Next(3, 15)
            };

            return View(model);
        }

        // ==============================================================
        // 2. USER DASHBOARD (Task-uri, XP și Insigne)
        // ==============================================================
        [Authorize]
        public async Task<IActionResult> UserDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index");

            // A. Încărcăm task-urile asignate active (care nu sunt finalizate/Status 2)
            var myTasks = await _db.TaskAssignments
                                   .Include(ta => ta.Task)
                                   .Where(ta => ta.UserId == user.Id && ta.Status != 2)
                                   .OrderBy(ta => ta.Task.DueDate)
                                   .ToListAsync();

            // B. Încărcăm INSIGNELE (Badges) pentru a le afișa pe profil
            var myBadges = await _db.UserBadges
                                    .Include(ub => ub.Badge)
                                    .Where(ub => ub.UserId == user.Id)
                                    .OrderByDescending(ub => ub.AwardedAt)
                                    .ToListAsync();

            // C. Trimitem datele către View prin ViewBag
            ViewBag.CurrentXp = user.TotalXp ?? 0;
            ViewBag.CurrentLevel = user.LevelId ?? 1;
            ViewBag.MyBadges = myBadges;

            return View(myTasks);
        }

        // ==============================================================
        // 3. ALTE PAGINI
        // ==============================================================

        public IActionResult ManagerDashboard()
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
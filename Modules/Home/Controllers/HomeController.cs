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
            var activeTasks = await _db.TaskAssignments.CountAsync(ta => ta.Status == 1);

            var totalXpNullable = await _db.UserXpHistories.SumAsync(x => x.ChangeAmount);
            int totalXp = totalXpNullable ?? 0;

            var recentActivity = await _db.UserXpHistories
                                            .Include(x => x.User)
                                            .OrderByDescending(x => x.CreatedAt)
                                            .Take(8)
                                            .ToListAsync();

            // B. Date REALE de Sistem (System Info)
            ViewBag.OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            ViewBag.FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            ViewBag.ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();

            // C. Data și Ora formatate pentru afișare modernă
            ViewBag.CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm");

            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalTasks = totalTasks,
                ActiveTasks = activeTasks,
                TotalXpAwarded = totalXp,
                RecentLogs = recentActivity,
                // Păstrăm simularea doar pentru graficele de performanță momentan (CPU/RAM necesită librării externe complexe)
                CpuUsagePercent = new Random().Next(20, 40),
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
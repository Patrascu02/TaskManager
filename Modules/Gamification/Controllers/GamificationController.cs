using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Modules.Users.Models;

namespace TaskManager.Modules.Gamification.Controllers
{
    [Authorize]
    public class GamificationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public GamificationController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ==========================================
        // LEADERBOARD (Top utilizatori)
        // ==========================================
        public async Task<IActionResult> Leaderboard()
        {
            var employees = await _userManager.GetUsersInRoleAsync("User");
            var leaderboard = employees
                                .OrderByDescending(u => u.TotalXp ?? 0)
                                .Take(10)
                                .ToList();
            return View(leaderboard);
        }

        // ==========================================
        // MY PROGRESS (Profilul de Gamification)
        // ==========================================
        public async Task<IActionResult> MyProgress()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            // Încărcăm utilizatorul cu tot cu istoricul de XP și Insigne
            // Folosim _db.Users pentru a putea da Include la relații
            var userData = await _db.Users
                                    .Include(u => u.XpHistories)
                                    .Include(u => u.Badges)
                                        .ThenInclude(ub => ub.Badge)
                                    .FirstOrDefaultAsync(u => u.Id == user.Id);

            return View(userData);
        }
    }
}
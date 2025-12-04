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

        public async Task<IActionResult> Leaderboard()
        {
            // 1. Luăm doar utilizatorii cu rolul de "User" (Angajații)
            var employees = await _userManager.GetUsersInRoleAsync("User");

            // 2. Îi sortăm în memorie (Memory Sort) după XP și luăm Top 10
            var leaderboard = employees
                                .OrderByDescending(u => u.TotalXp ?? 0) // Tratăm null ca 0
                                .Take(10)
                                .ToList();

            return View(leaderboard);
        }
    }
}
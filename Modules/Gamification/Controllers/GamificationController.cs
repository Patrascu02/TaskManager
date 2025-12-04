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
            // Luăm userii, îi sortăm după XP descrescător
            var users = await _userManager.Users
                                .OrderByDescending(u => u.TotalXp)
                                .Take(10) // Top 10
                                .ToListAsync();
            return View(users);
        }
    }
}
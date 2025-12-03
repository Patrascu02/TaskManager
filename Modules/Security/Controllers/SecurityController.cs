using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Data;
using TaskManager.Modules.Users.Models;

namespace TaskManager.Modules.Security.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SecurityController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public SecurityController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SystemStatus()
        {
            // 1. Colectăm statistici reale din Baza de Date
            var totalUsers = await _userManager.Users.CountAsync();
            var totalTasks = await _db.UserTasks.CountAsync();
            var activeTasks = await _db.UserTasks.CountAsync(t => t.IsActive);

            // 2. Trimitem datele către View prin ViewBag
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalTasks = totalTasks;
            ViewBag.ActiveTasks = activeTasks;
            ViewBag.ServerTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            ViewBag.OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            ViewBag.Framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

            return View();
        }
    }
}
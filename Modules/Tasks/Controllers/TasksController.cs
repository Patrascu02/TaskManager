using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Modules.Tasks.Models;
using TaskManager.Modules.Users.Models;

namespace TaskManager.Modules.Tasks.Controllers
{
    [Authorize(Roles = "Manager,Admin")] // Managerul este șeful aici
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // 1. MANAGE TASKS (Lista de task-uri)
        public async Task<IActionResult> Index()
        {
            // Încărcăm task-urile împreună cu cine le-a creat
            var tasks = await _db.UserTasks
                                 .OrderByDescending(t => t.DueDate)
                                 .ToListAsync();
            return View(tasks);
        }

        // 2. ASSIGN TASKS (Funcționează ca Create)
        public async Task<IActionResult> Assign()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");
            var model = new TaskCreateViewModel
            {
                DueDate = DateTime.Today.AddDays(3),
                UsersList = new MultiSelectList(users, "Id", "FullName")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _userManager.GetUsersInRoleAsync("User");
                model.UsersList = new MultiSelectList(users, "Id", "FullName", model.SelectedUserIds);
                return View(model);
            }

            // Salvare Task
            var newTask = new UserTask
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                Priority = (byte)model.Priority,
                IsActive = true,
                CreatedById = _userManager.GetUserId(User)
            };

            _db.UserTasks.Add(newTask);
            await _db.SaveChangesAsync();

            // Salvare Alocări
            if (model.SelectedUserIds != null)
            {
                foreach (var userId in model.SelectedUserIds)
                {
                    _db.TaskAssignments.Add(new TaskAssignment
                    {
                        TaskId = newTask.TaskId,
                        UserId = userId,
                        AssignedAt = DateTime.Now,
                        Status = 0 // Pending
                    });
                }
                await _db.SaveChangesAsync();
            }

            TempData["Success"] = "Task creat și alocat cu succes!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Tasks/Priorities
        public IActionResult Priorities()
        {
            return View();
        }

        // 3. DELETE TASK
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.UserTasks.FindAsync(id);
            if (task != null)
            {
                _db.UserTasks.Remove(task);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Task șters.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
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
    //  [Authorize] simplu permite oricui e logat (inclusiv User) să intre
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // =========================================================
        // ZONA MANAGER & ADMIN (Index, Create, Delete, Priorities)
        // =========================================================

        [Authorize(Roles = "Manager,Admin")] 
        public async Task<IActionResult> Index()
        {
            var tasks = await _db.UserTasks
                                 .OrderByDescending(t => t.DueDate)
                                 .ToListAsync();
            return View(tasks);
        }

        [Authorize(Roles = "Manager,Admin")] 
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
        [Authorize(Roles = "Manager,Admin")] 
        public async Task<IActionResult> Assign(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _userManager.GetUsersInRoleAsync("User");
                model.UsersList = new MultiSelectList(users, "Id", "FullName", model.SelectedUserIds);
                return View(model);
            }

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

            if (model.SelectedUserIds != null)
            {
                foreach (var userId in model.SelectedUserIds)
                {
                    _db.TaskAssignments.Add(new TaskAssignment
                    {
                        TaskId = newTask.TaskId,
                        UserId = userId,
                        AssignedAt = DateTime.Now,
                        Status = 0
                    });
                }
                await _db.SaveChangesAsync();
            }

            TempData["Success"] = "Task creat și alocat!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")] 
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

        public IActionResult Priorities()
        {
            return View();
        }

        // =========================================================
        // ZONA USER (Voting & Completion)
        // =========================================================

        [Authorize(Roles = "User")] 
        public async Task<IActionResult> VoteDifficulty(int id)
        {
            var assignment = await _db.TaskAssignments
                .Include(t => t.Task)
                .FirstOrDefaultAsync(a => a.TaskAssignmentId == id);

            if (assignment == null) return NotFound();
            return View(assignment);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SubmitVote(int assignmentId, int difficultyScore)
        {
            var existingVote = await _db.DifficultyVotes
                .FirstOrDefaultAsync(v => v.TaskAssignmentId == assignmentId);

            if (existingVote != null)
            {
                existingVote.VoteValue = (byte)difficultyScore;
                _db.Update(existingVote);
            }
            else
            {
                var vote = new DifficultyVote
                {
                    TaskAssignmentId = assignmentId,
                    VoteValue = (byte)difficultyScore
                };
                _db.DifficultyVotes.Add(vote);
            }

            var assignment = await _db.TaskAssignments.FindAsync(assignmentId);
            if (assignment != null && assignment.Status == 0)
            {
                assignment.Status = 1; // În Lucru
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = "Vot înregistrat!";

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var assignment = await _db.TaskAssignments
                .Include(t => t.Task)
                .FirstOrDefaultAsync(a => a.TaskAssignmentId == id);
            if (assignment == null) return NotFound();
            return View(assignment);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SubmitCompletion(int assignmentId, string feedbackText, int finalDifficulty)
        {
            var assignment = await _db.TaskAssignments.FindAsync(assignmentId);
            if (assignment == null) return NotFound();

            var initialVotes = await _db.DifficultyVotes
                                        .Where(v => v.TaskAssignmentId == assignmentId)
                                        .Select(v => (int)v.VoteValue)
                                        .ToListAsync();

            double initialAverage = initialVotes.Any() ? initialVotes.Average() : 2.0;
            int xpBase = 50;
            decimal feedbackFactor = 1.0m + ((decimal)finalDifficulty - (decimal)initialAverage) * 0.1m;
            if (feedbackFactor < 0.5m) feedbackFactor = 0.5m;
            int xpFinal = (int)(xpBase * feedbackFactor);

            var feedback = new CompletionFeedback
            {
                TaskAssignmentId = assignmentId,
                PerceivedOutcome = (byte)finalDifficulty,
                XpBase = xpBase,
                DifficultyAverage = (decimal)initialAverage,
                FeedbackFactor = feedbackFactor,
                XpFinal = xpFinal,
                CreatedAt = DateTime.Now
            };

            _db.CompletionFeedbacks.Add(feedback);

            assignment.Status = 2; // Completed
            assignment.CompletedAt = DateTime.Now;

            var user = await _userManager.FindByIdAsync(assignment.UserId);
            if (user != null)
            {
                user.TotalXp = (user.TotalXp ?? 0) + xpFinal;
                int newLevel = (user.TotalXp.Value / 100) + 1;
                if (newLevel > (user.LevelId ?? 1))
                {
                    user.LevelId = newLevel;
                    TempData["Info"] = $" Level Up! Ai ajuns la nivelul {newLevel}!";
                }
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Task finalizat! (+{xpFinal} XP)";
            return RedirectToAction("Index", "Home");
        }
    }
}
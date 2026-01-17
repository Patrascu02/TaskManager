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
        // ZONA MANAGER & ADMIN
        // =========================================================

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Index()
        {
            // Folosim .Assignments conform modelului tău UserTask
            var tasks = await _db.UserTasks
                                 .Include(t => t.Assignments)
                                    .ThenInclude(ta => ta.User)
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

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Reassign(int id)
        {
            var assignment = await _db.TaskAssignments
                                      .Include(t => t.Task)
                                      .Include(u => u.User)
                                      .FirstOrDefaultAsync(a => a.TaskAssignmentId == id);

            if (assignment == null) return NotFound();

            var users = await _userManager.GetUsersInRoleAsync("User");
            ViewBag.Users = new SelectList(users, "Id", "FullName", assignment.UserId);
            return View(assignment);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reassign(int id, string newUserId)
        {
            var assignment = await _db.TaskAssignments.FindAsync(id);
            if (assignment == null) return NotFound();

            assignment.UserId = newUserId;
            assignment.Status = 1; // Îl punem direct în lucru pentru noul user
            assignment.AssignedAt = DateTime.Now;

            _db.Update(assignment);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Task-ul a fost reasignat cu succes!";
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

        // =========================================================
        // ZONA USER (Gamification & Completion)
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
                _db.DifficultyVotes.Add(new DifficultyVote
                {
                    TaskAssignmentId = assignmentId,
                    VoteValue = (byte)difficultyScore
                });
            }

            var assignment = await _db.TaskAssignments.FindAsync(assignmentId);
            if (assignment != null && assignment.Status == 0)
            {
                assignment.Status = 1; // Trecem în statusul "În Lucru"
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
            var assignment = await _db.TaskAssignments
                                      .Include(a => a.Task)
                                      .FirstOrDefaultAsync(a => a.TaskAssignmentId == assignmentId);

            if (assignment == null) return NotFound();

            // 1. Calculăm media voturilor inițiale
            var initialVotes = await _db.DifficultyVotes
                                        .Where(v => v.TaskAssignmentId == assignmentId)
                                        .Select(v => (int)v.VoteValue)
                                        .ToListAsync();

            double initialAverage = initialVotes.Any() ? initialVotes.Average() : 2.0;
            int xpBase = 50;
            decimal feedbackFactor = 1.0m + ((decimal)finalDifficulty - (decimal)initialAverage) * 0.1m;
            if (feedbackFactor < 0.5m) feedbackFactor = 0.5m;

            int xpFinal = (int)(xpBase * feedbackFactor);

            // 2. Aplicăm PENALIZAREA DE 50% dacă termenul este depășit
            bool isOverdue = assignment.Task.DueDate.HasValue && DateTime.Now > assignment.Task.DueDate.Value;
            if (isOverdue)
            {
                xpFinal = xpFinal / 2;
                TempData["Warning"] = "Task finalizat după termen! Ai primit doar 50% din XP.";
            }

            // 3. Salvăm feedback-ul și XP-ul final
            _db.CompletionFeedbacks.Add(new CompletionFeedback
            {
                TaskAssignmentId = assignmentId,
                PerceivedOutcome = (byte)finalDifficulty,
                XpBase = xpBase,
                DifficultyAverage = (decimal)initialAverage,
                FeedbackFactor = feedbackFactor,
                XpFinal = xpFinal,
                CreatedAt = DateTime.Now
            });

            assignment.Status = 2; // Finalizat
            assignment.CompletedAt = DateTime.Now;

            // Închidem task-ul părinte dacă nu mai sunt alți useri care lucrează la el
            bool areOthersWorking = await _db.TaskAssignments
                .AnyAsync(ta => ta.TaskId == assignment.TaskId && ta.TaskAssignmentId != assignmentId && ta.Status != 2);

            if (!areOthersWorking && assignment.Task != null)
            {
                assignment.Task.IsActive = false;
            }

            // 4. Actualizăm profilul utilizatorului (XP & Level)
            var user = await _userManager.FindByIdAsync(assignment.UserId);
            if (user != null)
            {
                user.TotalXp = (user.TotalXp ?? 0) + xpFinal;

                _db.UserXpHistories.Add(new UserXpHistory
                {
                    UserId = user.Id,
                    ChangeAmount = xpFinal,
                    Reason = $"Finalizare Task: {assignment.Task?.Title} {(isOverdue ? "(Overdue)" : "")}",
                    CreatedAt = DateTime.Now
                });

                int newLevel = (user.TotalXp.Value / 100) + 1;
                if (newLevel > (user.LevelId ?? 1))
                {
                    user.LevelId = Math.Min(newLevel, 100);
                    TempData["Info"] = $"🎉 Felicitări! Ai ajuns la nivelul {user.LevelId}!";
                }

                // 5. LOGICA PENTRU INSIGNE (AwardedAt conform modelului tău)
                bool hasNoviceBadge = await _db.UserBadges.AnyAsync(ub => ub.UserId == user.Id && ub.BadgeId == 1);
                if (!hasNoviceBadge)
                {
                    _db.UserBadges.Add(new UserBadge
                    {
                        UserId = user.Id,
                        BadgeId = 1,
                        AwardedAt = DateTime.Now // Folosim AwardedAt din modelul tău
                    });
                    TempData["Info"] += " 🏆 Ai obținut insigna: Novice!";
                }
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Sarcina a fost închisă! (+{xpFinal} XP)";
            return RedirectToAction("Index", "Home");
        }
    }
}
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
                ManagerDifficulty = model.ManagerDifficulty, // AICI SALVĂM VOTUL SECRET
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
            var currentAssignment = await _db.TaskAssignments
                                             .Include(t => t.Task)
                                             .Include(u => u.User)
                                             .FirstOrDefaultAsync(a => a.TaskAssignmentId == id);

            if (currentAssignment == null) return NotFound();

            var existingUserIds = await _db.TaskAssignments
                                           .Where(ta => ta.TaskId == currentAssignment.TaskId)
                                           .Select(ta => ta.UserId)
                                           .ToListAsync();

            var allUsers = await _userManager.GetUsersInRoleAsync("User");

            var availableUsers = allUsers
                .Where(u => !existingUserIds.Contains(u.Id) || u.Id == currentAssignment.UserId)
                .ToList();

            ViewBag.Users = new SelectList(availableUsers, "Id", "FullName", currentAssignment.UserId);

            return View(currentAssignment);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reassign(int id, string newUserId)
        {
            var assignment = await _db.TaskAssignments.FindAsync(id);
            if (assignment == null) return NotFound();

            assignment.UserId = newUserId;
            assignment.Status = 1;
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

        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Priorities()
        {
            return View();
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
                assignment.Status = 1;
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

            // 1. CALCUL XP: 50 - |Diferența|

            // A. Valorile brute (1-5)
            int managerVote = assignment.Task.ManagerDifficulty;
            int userVote = finalDifficulty;

            // B. Convertim în XP (1=30 ... 5=50)
            int managerXp = 30 + (managerVote - 1) * 5;
            int userXp = 30 + (userVote - 1) * 5;

            // C. Calculăm Diferența (Discrepanța)
            int diff = Math.Abs(managerXp - userXp);

            // D. Formula Finală
            int xpFinal = 50 - diff;

            // E. Mesaj Feedback
            if (diff == 0)
            {
                TempData["Success"] = $"Sincronizare perfectă! Ai primit maximul de {xpFinal} XP.";
            }
            else
            {
                TempData["Info"] = $"XP Bază (50) - Diferență Opinie ({diff}) = {xpFinal} XP.";
            }

            // 2. Penalizare Termen (Overdue)
            bool isOverdue = assignment.Task.DueDate.HasValue && DateTime.Now > assignment.Task.DueDate.Value;
            if (isOverdue)
            {
                xpFinal = xpFinal / 2;
                TempData["Warning"] = "Task expirat! XP înjumătățit.";
            }

            // 3. Salvare date
            _db.CompletionFeedbacks.Add(new CompletionFeedback
            {
                TaskAssignmentId = assignmentId,
                PerceivedOutcome = (byte)userVote,
                XpBase = 50,
                DifficultyAverage = (decimal)managerVote,
                FeedbackFactor = 1.0m,
                XpFinal = xpFinal,
                CreatedAt = DateTime.Now
            });

            assignment.Status = 2; // Finalizat
            assignment.CompletedAt = DateTime.Now;

            // Închidere task părinte
            bool areOthersWorking = await _db.TaskAssignments
                .AnyAsync(ta => ta.TaskId == assignment.TaskId && ta.TaskAssignmentId != assignmentId && ta.Status != 2);

            if (!areOthersWorking && assignment.Task != null)
            {
                assignment.Task.IsActive = false;
            }

            // 4. Update Profil User
            var user = await _userManager.FindByIdAsync(assignment.UserId);
            if (user != null)
            {
                user.TotalXp = (user.TotalXp ?? 0) + xpFinal;

                _db.UserXpHistories.Add(new UserXpHistory
                {
                    UserId = user.Id,
                    ChangeAmount = xpFinal,
                    Reason = $"Task: {assignment.Task?.Title} (M:{managerVote} vs U:{userVote})",
                    CreatedAt = DateTime.Now
                });

                int newLevel = (user.TotalXp.Value / 100) + 1;
                if (newLevel > (user.LevelId ?? 1))
                {
                    user.LevelId = Math.Min(newLevel, 100);
                    TempData["Success"] += $" LEVEL UP -> {user.LevelId}!";
                }

                // Insigne
                bool hasNoviceBadge = await _db.UserBadges.AnyAsync(ub => ub.UserId == user.Id && ub.BadgeId == 1);
                if (!hasNoviceBadge && user.TotalXp >= 100)
                {
                    _db.UserBadges.Add(new UserBadge { UserId = user.Id, BadgeId = 1, AwardedAt = DateTime.Now });
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
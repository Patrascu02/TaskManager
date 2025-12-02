using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Modules.Users.Models;

namespace TaskManager.Modules.Users.Controllers
{
    [Authorize(Roles = "Admin")] // doar Admin poate accesa
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        // ====================================
        // INDEX: listare + cautare + sort + paginare
        // ====================================
        public async Task<IActionResult> Index(string search, string sort = "username", int page = 1, int pageSize = 10)
        {
            var q = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(u => u.UserName.Contains(search) || u.Email.Contains(search) || u.FullName.Contains(search));
            }

            q = sort switch
            {
                "email" => q.OrderBy(u => u.Email),
                "email_desc" => q.OrderByDescending(u => u.Email),
                "username_desc" => q.OrderByDescending(u => u.UserName),
                _ => q.OrderBy(u => u.UserName)
            };

            var total = await q.CountAsync();
            var users = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var items = new List<UserListItemViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                items.Add(new UserListItemViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FullName = u.FullName,
                    Roles = roles.ToArray()
                });
            }

            var vm = new UserListViewModel
            {
                Users = items,
                Search = search,
                Sort = sort,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return View(vm);
        }

        // ====================================
        // GET: Create
        // ====================================
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var vm = new UserCreateViewModel
            {
                AvailableRoles = roles
            };
            return View(vm);
        }

        // ====================================
        // POST: Create
        // ====================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));

                await _userManager.AddToRoleAsync(user, model.Role);
            }

            TempData["Success"] = "User creat cu succes.";
            return RedirectToAction(nameof(Index));
        }

        // ====================================
        // GET: Edit
        // ====================================
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var vm = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                AvailableRoles = roles,
                CurrentRoles = userRoles.ToArray()
            };

            return View(vm);
        }

        // ====================================
        // POST: Edit
        // ====================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.Email = model.Email;
            user.UserName = model.Email;
            user.FullName = model.FullName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var e in updateResult.Errors) ModelState.AddModelError("", e.Description);
                model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            // Actualizare roluri
            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            if (!string.IsNullOrEmpty(model.Role))
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));

                await _userManager.AddToRoleAsync(user, model.Role);
            }

            TempData["Success"] = "Modificările au fost salvate.";
            return RedirectToAction(nameof(Index));
        }

        // ====================================
        // POST: Delete
        // ====================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var res = await _userManager.DeleteAsync(user);
            if (!res.Succeeded)
            {
                TempData["Error"] = "Ștergere eșuată.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "User șters.";
            return RedirectToAction(nameof(Index));
        }

        // ====================================
        // POST: ResetPassword
        // ====================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPass = "Tmp!" + Guid.NewGuid().ToString("N").Substring(0, 8);
            var result = await _userManager.ResetPasswordAsync(user, token, newPass);

            if (!result.Succeeded)
                TempData["Error"] = "Reset parola eșuat.";
            else
                TempData["Info"] = $"Parola temporară: {newPass}";

            return RedirectToAction(nameof(Index));
        }
    }
}

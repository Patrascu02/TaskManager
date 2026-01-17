using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TaskManager.Modules.Users.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace TaskManager.Modules.Users.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 1. INDEX (LISTA)
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var modelList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                modelList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "User",
                    Password = "Hidden" // Nu afișăm parola
                });
            }
            return View(modelList);
        }

        // 2. CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            ModelState.Remove("Id"); // Id-ul nu e necesar la creare

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = $"{model.FirstName} {model.LastName}",
                    LevelId = 1,
                    TotalXp = 0
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }
                    await _userManager.AddToRoleAsync(user, model.Role);
                    TempData["Success"] = "User creat cu succes!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // 4. EDIT (GET)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "User"
            };
            return View(model);
        }

        // 5. EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.FullName = $"{model.FirstName} {model.LastName}";
                user.Email = model.Email;
                user.UserName = model.Email;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resultPass = await _userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!resultPass.Succeeded)
                    {
                        foreach (var error in resultPass.Errors) ModelState.AddModelError("", error.Description);
                        return View(model);
                    }
                }

                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                if (!await _roleManager.RoleExistsAsync(model.Role))
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));

                await _userManager.AddToRoleAsync(user, model.Role);
                await _userManager.UpdateAsync(user);

                TempData["Success"] = "User actualizat!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 6. DELETE
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // 7. VALIDARE LIVE
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Json(true);
            return Json($"Adresa de email '{email}' este deja folosită.");
        }
    }
}
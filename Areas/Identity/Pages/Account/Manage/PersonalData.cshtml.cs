using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Modules.Users.Models; // Asigură-te că ai namespace-ul corect pentru ApplicationUser

namespace TaskManager.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonalDataModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Proprietăți pentru afișare
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; } // Adaugă asta dacă ai câmpul în ApplicationUser
        public bool IsEmailConfirmed { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Populăm datele
            Username = await _userManager.GetUserNameAsync(user);
            Email = await _userManager.GetEmailAsync(user);
            PhoneNumber = await _userManager.GetPhoneNumberAsync(user);
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            // Dacă ai proprietatea FullName în modelul ApplicationUser:
            FullName = user.FullName;

            return Page();
        }
    }
}
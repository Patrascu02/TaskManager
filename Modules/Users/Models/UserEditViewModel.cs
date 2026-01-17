using System.ComponentModel.DataAnnotations;

namespace TaskManager.Modules.Users.Models
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [Display(Name = "Prenume")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Numele de familie este obligatoriu")]
        [Display(Name = "Nume de Familie")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Emailul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Format invalid")]
        public string Email { get; set; }

        [Display(Name = "Parolă Nouă (Opțional)")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Parola trebuie să aibă minim 6 caractere.")]
        public string? Password { get; set; } // Nullable = Opțional la editare

        [Required]
        public string Role { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Modules.Users.Models
{
    // Clasa PRINCIPALĂ pe care o caută Controller-ul și Index-ul
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [Display(Name = "Prenume")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Numele de familie este obligatoriu")]
        [Display(Name = "Nume de Familie")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Emailul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Format invalid")]
        [Remote(action: "IsEmailAvailable", controller: "Users", ErrorMessage = "Acest email este deja utilizat.")]
        public string Email { get; set; } = "";

        // Parola e string simplu, nu null, pentru creare
        [Required(ErrorMessage = "Parola este obligatorie")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Minim 6 caractere.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        public string Role { get; set; } = "User";
    }

    // Clasa pentru EDITARE (folosită în metoda Edit)
    public class UserEditViewModel
    {
        public string Id { get; set; } = "";

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Numele de familie este obligatoriu")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Emailul este obligatoriu")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Display(Name = "Parolă Nouă (Opțional)")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Minim 6 caractere.")]
        public string? Password { get; set; } // Nullable = opțional

        [Required]
        public string Role { get; set; } = "User";
    }
}
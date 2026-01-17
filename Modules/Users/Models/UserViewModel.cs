using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Modules.Users.Models
{
    // Modelul principal pentru Listare și Creare
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [Display(Name = "Prenume")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Numele de familie este obligatoriu")]
        [Display(Name = "Nume de Familie")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Emailul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Format invalid")]
        // Validare Remote (verificare live dacă emailul există)
        [Remote(action: "IsEmailAvailable", controller: "Users", ErrorMessage = "Acest email este deja utilizat.")]
        public string Email { get; set; }

        // Parola este obligatorie la creare
        [Required(ErrorMessage = "Parola este obligatorie")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Minim 6 caractere.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

    // Modelul pentru Editare (unde parola e opțională)
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Numele de familie este obligatoriu")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Emailul este obligatoriu")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Parolă Nouă (Opțional)")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Minim 6 caractere.")]
        public string? Password { get; set; } // Nullable, deci nu e obligatoriu

        [Required]
        public string Role { get; set; }
    }
}
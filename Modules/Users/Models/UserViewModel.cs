using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Modules.Gamification.Models;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Users.Models
{
    public class UserListItemViewModel
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string[] Roles { get; set; } = new string[0];
    }

    public class UserListViewModel
    {
        public IEnumerable<UserListItemViewModel> Users { get; set; } = new List<UserListItemViewModel>();
        public string? Search { get; set; }
        public string Sort { get; set; } = "username";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
    }

    public class UserCreateViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        public string? Role { get; set; }
        public IEnumerable<string>? AvailableRoles { get; set; }
    }

    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        public string? Role { get; set; }
        public string[] CurrentRoles { get; set; } = new string[0];
        public IEnumerable<string>? AvailableRoles { get; set; }
    }
}

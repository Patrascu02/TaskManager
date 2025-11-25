using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class AdminViewModel
    {
        public string? FullName { get; set; }
        public IEnumerable<ApplicationUser>? AllUsers { get; set; }
        public IEnumerable<UserTask>? AllTasks { get; set; }
        public IEnumerable<UserBadge>? AllBadges { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TaskManager.Modules.Gamification.Models;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Users.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Păstrăm FullName pentru compatibilitate, dar adăugăm câmpurile noi
        public string? FullName { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        public int? TotalXp { get; set; } = 0;

        public int? LevelId { get; set; }
        [ForeignKey("LevelId")]
        public Level? Level { get; set; }

        public bool? IsActive { get; set; } = true;

        // Relații
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
        public ICollection<UserXpHistory> XpHistories { get; set; }
        public ICollection<UserBadge> Badges { get; set; }
        public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; }
    }
}
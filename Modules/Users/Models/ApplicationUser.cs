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
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? FullName { get; set; }

        public int? TotalXp { get; set; } = 0;

        // --- LEVEL ---
        public int? LevelId { get; set; }
        [ForeignKey("LevelId")]
        public Level? Level { get; set; }

        // --- BADGE (Aici era problema) ---
        public int? CurrentBadgeId { get; set; }

        [ForeignKey("CurrentBadgeId")]
        public virtual Badge? CurrentBadge { get; set; } // <--- ACEASTA LINIE LIPSEA

        public bool? IsActive { get; set; } = true;

        // Relații
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
        public ICollection<UserXpHistory> XpHistories { get; set; }
        public ICollection<UserBadge> Badges { get; set; }
        public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; }
    }
}
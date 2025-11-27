using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Modules.Gamification.Models;

namespace TaskManager.Modules.Users.Models
{
    public class UserBadge
    {
        [Key]
        public int? UserBadgeId { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int? BadgeId { get; set; }
        [ForeignKey("BadgeId")]
        public Badge Badge { get; set; }

        [Required]
        public DateTime AwardedAt { get; set; } = DateTime.Now;
    }
}

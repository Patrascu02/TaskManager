using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Modules.Users.Models;

namespace TaskManager.Modules.Gamification.Models
{
    public class Badge
    {
        [Key]
        public int? BadgeId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public ICollection<UserBadge> UserBadges { get; set; }
    }
}

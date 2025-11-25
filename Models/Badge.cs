using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
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

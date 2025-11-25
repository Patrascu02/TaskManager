using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class LeaderboardEntry
    {
        [Key]
        public int? LeaderboardEntryId { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public string? PeriodType { get; set; } // Weekly, Monthly

        [Required]
        public DateTime? PeriodStart { get; set; }

        [Required]
        public DateTime? PeriodEnd { get; set; }

        [Required]
        public int? XpTotal { get; set; }

        [Required]
        public int? Rank { get; set; }
    }
}

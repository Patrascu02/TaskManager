using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class UserXpHistory
    {
        [Key]
        public int? UserXpHistoryId { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int? TaskAssignmentId { get; set; } // optional
        // [ForeignKey("TaskAssignmentId")]
        // public TaskAssignment TaskAssignment { get; set; }

        [Required]
        public int? ChangeAmount { get; set; }

        [Required]
        public string? Reason { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}

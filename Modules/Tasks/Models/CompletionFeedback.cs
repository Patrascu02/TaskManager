using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Modules.Tasks.Models
{
    public class CompletionFeedback
    {
        [Key]
        public int? CompletionFeedbackId { get; set; }

        [Required]
        public int? TaskAssignmentId { get; set; }
        [ForeignKey("TaskAssignmentId")]
        public TaskAssignment TaskAssignment { get; set; }

        [Required]
        public byte PerceivedOutcome { get; set; }

        [Required]
        public int? XpBase { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal DifficultyAverage { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal FeedbackFactor { get; set; }

        [Required]
        public int XpFinal { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

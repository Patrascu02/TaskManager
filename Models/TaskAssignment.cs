using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class TaskAssignment
    {
        [Key]
        public int TaskAssignmentId { get; set; }

        [Required]
        public int? TaskId { get; set; }
        [ForeignKey("TaskId")]
        public UserTask ?Task { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime AssignedAt { get; set; }

        [Required]
        public byte Status { get; set; } = 0;

        public DateTime? CompletedAt { get; set; }

        public ICollection<DifficultyVote> ?DifficultyVotes { get; set; }
        public ICollection<CompletionFeedback> ?CompletionFeedbacks { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class DifficultyVote
    {
        [Key]
        public int? DifficultyVoteId { get; set; }

        [Required]
        public int? TaskAssignmentId { get; set; }
        [ForeignKey("TaskAssignmentId")]
        public TaskAssignment TaskAssignment { get; set; }

        [Required]
        public byte VoteValue { get; set; }
    }
}

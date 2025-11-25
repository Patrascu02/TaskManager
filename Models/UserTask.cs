using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class UserTask
    {
        [Key]
        public int? TaskId { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? CreatedById { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public byte Priority { get; set; } = 2;

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<TaskAssignment> Assignments { get; set; }
    }
}

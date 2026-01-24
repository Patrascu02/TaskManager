using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Modules.Users.Models; // Asigură-te că ai acest using pentru ApplicationUser

namespace TaskManager.Modules.Tasks.Models
{
    public class UserTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        // PRIORITATE (Urgență): 1=Critical, 2=Medium, 3=Normal
        [Required]
        public byte Priority { get; set; } = 2;

        // --- CÂMP NOU: DIFICULTATE MANAGER (Vot Secret 1-5) ---
        // Folosit pentru calculul XP la final: 50 - |ManagerDifficulty - UserVote|
        public int ManagerDifficulty { get; set; } = 1;

        [Required]
        public bool IsActive { get; set; } = true;

        // Relația cu cel care a creat task-ul (Managerul)
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public ApplicationUser? CreatedBy { get; set; }

        // Relația cu asignările (Cine lucrează la task)
        public ICollection<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>();
    }
}
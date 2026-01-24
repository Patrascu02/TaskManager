using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManager.Modules.Tasks.Models
{
    public class TaskCreateViewModel
    {
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Descrierea este obligatorie")]
        public string Description { get; set; } = "";

        [Required]
        public DateTime DueDate { get; set; }

        // URGENȚA
        [Required]
        public int Priority { get; set; }

        // DIFICULTATEA (XP MANAGER)
        [Required]
        [Range(1, 5, ErrorMessage = "Alege o dificultate estimată (1-5)")]
        public int ManagerDifficulty { get; set; }

        public List<string> SelectedUserIds { get; set; } = new List<string>();
        public MultiSelectList? UsersList { get; set; }
    }
}
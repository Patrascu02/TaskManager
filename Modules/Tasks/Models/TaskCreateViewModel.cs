using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManager.Modules.Tasks.Models
{
    public class TaskCreateViewModel
    {
        [Required(ErrorMessage = "Te rog introdu un titlu.")]
        [Display(Name = "Titlu Task")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Descriere")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Selectează o dată limită.")]
        [Display(Name = "Termen Limită")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Prioritate")]
        public int Priority { get; set; } = 2; // 2 = Medium (Default)

        // --- PARTEA DE ALOCARE ---

        [Display(Name = "Asignează către")]
        public List<string> SelectedUserIds { get; set; } = new List<string>();

        // Aceasta este lista care populează checkbox-urile din View
        public MultiSelectList? UsersList { get; set; }
    }
}
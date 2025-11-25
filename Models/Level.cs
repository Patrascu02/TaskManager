using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class Level
    {
        [Key]
        public int? LevelId { get; set; }

        [Required]
        public string? LevelName { get; set; }

        [Required]
        public int? MinXp { get; set; }

        [Required]
        public int? MaxXp { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}

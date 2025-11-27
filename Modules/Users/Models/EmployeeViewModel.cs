using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Modules.Gamification.Models;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Users.Models
{
    public class EmployeeViewModel
    {
        public string? FullName { get; set; }
        public int? TotalXp { get; set; }
        public Level? Level { get; set; }
        public IEnumerable<TaskAssignment> TaskAssignments { get; set; }
        public IEnumerable<UserBadge> Badges { get; set; }
    }
}

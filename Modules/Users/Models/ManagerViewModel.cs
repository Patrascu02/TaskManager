using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Users.Models
{
    public class ManagerViewModel
    {
        public string? FullName { get; set; }
        public IEnumerable<UserTask> CreatedTasks { get; set; }
        public IEnumerable<TaskAssignment> TaskAssignments { get; set; }
        public IEnumerable<UserXpHistory> TeamXpHistories { get; set; }
    }
}

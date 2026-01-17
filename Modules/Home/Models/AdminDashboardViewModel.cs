using System.Collections.Generic;
using TaskManager.Modules.Users.Models; // Pentru UserXpHistory

namespace TaskManager.Modules.Home.Models
{
    public class AdminDashboardViewModel
    {
        // 1. Statistici Generale
        public int TotalUsers { get; set; }
        public int TotalTasks { get; set; }
        public int ActiveTasks { get; set; }
        public int TotalXpAwarded { get; set; }

        // 2. Monitorizare Sistem (Server)
        public int CpuUsagePercent { get; set; }
        public double RamUsageMb { get; set; }
        public double RamTotalMb { get; set; }
        public int ActiveSessions { get; set; }

        // Proprietăți calculate pentru Progress Bars
        public int RamPercentage => RamTotalMb > 0 ? (int)((RamUsageMb / RamTotalMb) * 100) : 0;

        // 3. Lista de Activitate Recentă (Log-uri XP)
        public List<UserXpHistory> RecentLogs { get; set; } = new List<UserXpHistory>();
    }
}
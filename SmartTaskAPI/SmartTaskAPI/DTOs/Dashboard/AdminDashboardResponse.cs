namespace SmartTaskAPI.DTOs.Dashboard;

public class AdminDashboardResponse
{
    public int TotalUsers { get; set; }
    public int TotalTasks { get; set; }
    public int PendingTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int InProgressTasks { get; set; }
}
namespace SmartTaskAPI.DTOs.Tasks;

public class TaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending"; // Default status is Pending
    public DateTime DueDate { get; set; } = DateTime.UtcNow;
    public int? AssignedToId { get; set; } //nullable
}
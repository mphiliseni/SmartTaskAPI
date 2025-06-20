using SmartTaskAPI.DTOs.Tasks;


namespace SmartTaskAPI.DTOs.Tasks;

public class TaskResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; } = "Pending"; // Default status is Pending
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
}
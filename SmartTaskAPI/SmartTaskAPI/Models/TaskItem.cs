using System.ComponentModel.DataAnnotations;

namespace SmartTaskAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, InProgress, Done
        public DateTime DueDate { get; set; }

        public int? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }
}

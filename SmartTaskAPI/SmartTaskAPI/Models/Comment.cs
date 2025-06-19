using System.ComponentModel.DataAnnotations;

namespace SmartTaskAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int AuthorId { get; set; }
        public User? Author { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}

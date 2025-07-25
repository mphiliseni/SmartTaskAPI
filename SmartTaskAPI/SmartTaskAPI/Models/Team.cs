namespace SmartTaskAPI.Models;

public class Team
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<User>? Members { get;  set; }
}
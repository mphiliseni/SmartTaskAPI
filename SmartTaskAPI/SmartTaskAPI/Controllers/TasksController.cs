using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskAPI.Data;
using SmartTaskAPI.DTOs.Tasks;
using SmartTaskAPI.Models;

namespace SmartTaskAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAll()
    {
        var tasks = await _context.Tasks
                .Include(t => t.AssignedTo)
                .Select(t => new TaskResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    DueDate = t.DueDate,
                    AssignedTo = t.AssignedTo != null ? t.AssignedTo.FullName : null
                })
                .ToListAsync();

        return Ok(tasks);
    }
    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetById(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return NotFound();

        var result = new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            AssignedTo = task.AssignedTo?.FullName
        };

        return Ok(result);
    }
    // POST: api/tasks
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TaskResponse>> Create(TaskRequest request)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate,
            AssignedToId = request.AssignedToId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            AssignedTo = null
        });
    }
    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, TaskRequest request)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.DueDate = request.DueDate;
        task.AssignedToId = request.AssignedToId;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

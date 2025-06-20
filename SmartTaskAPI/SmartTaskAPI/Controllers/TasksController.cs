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
    [HttpGet]
public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? status = null,
    [FromQuery] string? search = null)
{
    var query = _context.Tasks
        .Include(t => t.AssignedTo)
        .AsQueryable();

    // search filter
    if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(t => t.Title.Contains(search));

    // status filter
    if (!string.IsNullOrWhiteSpace(status))
        query = query.Where(t => t.Status == status);

    //Pagination
    var totalCount = await query.CountAsync();
    var tasks = await query
        .OrderByDescending(t => t.DueDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var results = tasks.Select(t => new TaskResponse
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status,
        DueDate = t.DueDate,
        AssignedTo = t.AssignedTo?.FullName
    });

    return Ok(new
    {
        Total = totalCount,
        Page = page,
        PageSize = pageSize,
        Results = results
    });
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

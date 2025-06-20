using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskAPI.Data;
using SmartTaskAPI.DTOs.Dashboard;


namespace SmartTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/dashboard/admin
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminDashboardResponse>> GetAdminDashboard()
        {
            var response = new AdminDashboardResponse
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalTasks = await _context.Tasks.CountAsync(),
                PendingTasks = await _context.Tasks.CountAsync(t => t.Status == "Pending"),
                CompletedTasks = await _context.Tasks.CountAsync(t => t.Status == "Completed"),
                InProgressTasks = await _context.Tasks.CountAsync(t => t.Status == "In Progress")
            };

            return Ok(response);
        }
    }
    }
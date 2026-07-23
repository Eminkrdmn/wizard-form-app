using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;

namespace WizardFormApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedCheckController : ControllerBase
{
    private readonly AppDbContext _context;

    public SeedCheckController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var departments = await _context.Departments.Select(d => new { d.Id, d.Name, d.Code }).ToListAsync();
        var roles = await _context.Roles.Include(r => r.Department).Select(r => new { r.Id, r.Name, r.Code, r.Level, Department = r.Department.Name }).ToListAsync();
        var users = await _context.Users.Include(u => u.Role).Select(u => new { u.Id, u.Username, u.DisplayName, Role = u.Role.Name }).ToListAsync();

        return Ok(new
        {
            DepartmentCount = departments.Count,
            RoleCount = roles.Count,
            UserCount = users.Count,
            Departments = departments,
            Roles = roles,
            Users = users
        });
    }
}

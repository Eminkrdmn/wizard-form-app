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
        var forms = await _context.Forms.Select(f => new { f.Id, f.Name, f.IsSystemForm }).ToListAsync();
        var workflows = await _context.WorkflowDefinitions.Select(w => new { w.Id, w.Name, w.Code, w.Category }).ToListAsync();
        var steps = await _context.WorkflowSteps.Include(s => s.WorkflowDefinition).Include(s => s.AssignedRole)
            .Select(s => new { s.Id, Workflow = s.WorkflowDefinition.Name, s.StepOrder, s.Name, Role = s.AssignedRole.Name, s.ActionType, s.AssignmentRule, s.IsConditional, s.ConditionJson }).ToListAsync();

        return Ok(new
        {
            DepartmentCount = departments.Count,
            RoleCount = roles.Count,
            UserCount = users.Count,
            FormCount = forms.Count,
            WorkflowCount = workflows.Count,
            StepCount = steps.Count,
            Departments = departments,
            Roles = roles,
            Users = users,
            Forms = forms,
            Workflows = workflows,
            Steps = steps
        });
    }
}

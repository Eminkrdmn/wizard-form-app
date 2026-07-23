using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;

namespace WizardFormApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkflowsController : ControllerBase
{
    private readonly AppDbContext _context;

    public WorkflowsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? category)
    {
        var query = _context.WorkflowDefinitions
            .Where(w => w.IsActive);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(w => w.Category == category);

        var workflows = await query
            .Select(w => new WorkflowListDto
            {
                Id = w.Id,
                Name = w.Name,
                Code = w.Code,
                Category = w.Category,
                Description = w.Description,
                IsPredefined = w.IsPredefined,
                StepCount = w.Steps.Count,
                FormTemplateId = w.FormTemplateId
            })
            .OrderBy(w => w.Category)
            .ThenBy(w => w.Name)
            .ToListAsync();

        return Ok(workflows);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var workflow = await _context.WorkflowDefinitions
            .Where(w => w.Id == id)
            .Select(w => new WorkflowDetailDto
            {
                Id = w.Id,
                Name = w.Name,
                Code = w.Code,
                Category = w.Category,
                Description = w.Description,
                IsPredefined = w.IsPredefined,
                FormTemplateId = w.FormTemplateId,
                FormFieldsJson = w.FormTemplate != null ? w.FormTemplate.FieldsJson : null,
                Steps = w.Steps
                    .OrderBy(s => s.StepOrder)
                    .Select(s => new WorkflowStepDto
                    {
                        Id = s.Id,
                        StepOrder = s.StepOrder,
                        Name = s.Name,
                        ActionType = s.ActionType,
                        AssignedRoleName = s.AssignedRole.Name,
                        IsConditional = s.IsConditional,
                        ConditionJson = s.ConditionJson,
                        AssignmentRule = s.AssignmentRule
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        if (workflow == null)
            return NotFound(new { message = "Süreç tanımı bulunamadı" });

        return Ok(workflow);
    }
}

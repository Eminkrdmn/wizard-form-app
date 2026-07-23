using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;
using WizardFormApi.Models;

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
                        AssignedRoleId = s.AssignedRoleId,
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkflowRequest request)
    {
        if (request.Steps.Count == 0)
            return BadRequest(new { message = "En az bir adım gereklidir" });

        var code = request.Name
            .ToUpperInvariant()
            .Replace(" ", "_")
            .Replace("İ", "I")
            .Replace("Ö", "O")
            .Replace("Ü", "U")
            .Replace("Ş", "S")
            .Replace("Ç", "C")
            .Replace("Ğ", "G");

        if (await _context.WorkflowDefinitions.AnyAsync(w => w.Code == code))
            code = $"{code}_{DateTime.UtcNow.Ticks % 100000}";

        var workflow = new WorkflowDefinition
        {
            Name = request.Name,
            Code = code,
            Category = request.Category,
            Description = request.Description,
            IsPredefined = false,
            FormTemplateId = request.FormTemplateId
        };

        _context.WorkflowDefinitions.Add(workflow);
        await _context.SaveChangesAsync();

        var steps = request.Steps.Select(s => new WorkflowStep
        {
            WorkflowDefinitionId = workflow.Id,
            StepOrder = s.StepOrder,
            Name = s.Name,
            AssignedRoleId = s.AssignedRoleId,
            ActionType = s.ActionType
        }).ToList();

        _context.WorkflowSteps.AddRange(steps);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = workflow.Id }, await GetWorkflowDetail(workflow.Id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkflowRequest request)
    {
        var workflow = await _context.WorkflowDefinitions
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (workflow == null)
            return NotFound(new { message = "Süreç tanımı bulunamadı" });

        if (workflow.IsPredefined)
            return BadRequest(new { message = "Sistem süreçleri düzenlenemez" });

        if (request.Steps.Count == 0)
            return BadRequest(new { message = "En az bir adım gereklidir" });

        workflow.Name = request.Name;
        workflow.Description = request.Description;
        workflow.Category = request.Category;
        workflow.FormTemplateId = request.FormTemplateId;

        _context.WorkflowSteps.RemoveRange(workflow.Steps);

        var newSteps = request.Steps.Select(s => new WorkflowStep
        {
            WorkflowDefinitionId = workflow.Id,
            StepOrder = s.StepOrder,
            Name = s.Name,
            AssignedRoleId = s.AssignedRoleId,
            ActionType = s.ActionType
        }).ToList();

        _context.WorkflowSteps.AddRange(newSteps);
        await _context.SaveChangesAsync();

        return Ok(await GetWorkflowDetail(workflow.Id));
    }

    private async Task<WorkflowDetailDto?> GetWorkflowDetail(int id)
    {
        return await _context.WorkflowDefinitions
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
                        AssignedRoleId = s.AssignedRoleId,
                        IsConditional = s.IsConditional,
                        ConditionJson = s.ConditionJson,
                        AssignmentRule = s.AssignmentRule
                    }).ToList()
            })
            .FirstOrDefaultAsync();
    }
}

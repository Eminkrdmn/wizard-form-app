using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;
using WizardFormApi.Models;

namespace WizardFormApi.Services;

public class WorkflowEngine
{
    private readonly AppDbContext _context;

    public WorkflowEngine(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProcessDetailDto?> StartProcessAsync(StartProcessRequest request, int userId)
    {
        var workflow = await _context.WorkflowDefinitions
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.Id == request.WorkflowDefinitionId && w.IsActive);

        if (workflow == null)
            return null;

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return null;

        var firstStep = FindNextStep(workflow.Steps, 0, request.FormDataJson);
        if (firstStep == null)
            return null;

        var process = new ProcessInstance
        {
            WorkflowDefinitionId = workflow.Id,
            FormId = workflow.FormTemplateId,
            DataJson = request.FormDataJson,
            Status = "Aktif",
            CurrentStepOrder = firstStep.StepOrder,
            CreatedByUserId = userId
        };

        _context.Processes.Add(process);
        await _context.SaveChangesAsync();

        var assignedRoleId = ResolveRoleId(firstStep, user);

        var workItem = new WorkItem
        {
            ProcessInstanceId = process.Id,
            WorkflowStepId = firstStep.Id,
            AssignedToRoleId = assignedRoleId,
            Status = "Beklemede"
        };

        var history = new ProcessHistory
        {
            ProcessInstanceId = process.Id,
            Action = "Süreç Başlatıldı",
            Comment = $"{workflow.Name} süreci başlatıldı",
            PerformedByUserId = userId
        };

        _context.WorkItems.Add(workItem);
        _context.ProcessHistories.Add(history);
        await _context.SaveChangesAsync();

        return await GetProcessDetailAsync(process.Id);
    }

    public async Task<ProcessDetailDto?> ApproveStepAsync(int workItemId, int userId, string? notes)
    {
        var workItem = await _context.WorkItems
            .Include(w => w.Process)
                .ThenInclude(p => p.WorkflowDefinition)
                    .ThenInclude(wd => wd.Steps)
            .Include(w => w.WorkflowStep)
            .FirstOrDefaultAsync(w => w.Id == workItemId);

        if (workItem == null || workItem.Status != "Beklemede")
            return null;

        var process = workItem.Process;
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return null;

        workItem.Status = "Onaylandı";
        workItem.CompletedAt = DateTime.UtcNow;
        workItem.CompletedByUserId = userId;
        workItem.Notes = notes;

        _context.ProcessHistories.Add(new ProcessHistory
        {
            ProcessInstanceId = process.Id,
            Action = $"{workItem.WorkflowStep.Name} — Onaylandı",
            Comment = notes,
            PerformedByUserId = userId
        });

        var allSteps = process.WorkflowDefinition.Steps.OrderBy(s => s.StepOrder).ToList();
        var nextStep = FindNextStep(allSteps, workItem.WorkflowStep.StepOrder, process.DataJson);

        if (nextStep == null)
        {
            process.Status = "Tamamlandı";
            process.CompletedAt = DateTime.UtcNow;

            _context.ProcessHistories.Add(new ProcessHistory
            {
                ProcessInstanceId = process.Id,
                Action = "Süreç Tamamlandı",
                Comment = "Tüm adımlar onaylandı",
                PerformedByUserId = userId
            });
        }
        else
        {
            process.CurrentStepOrder = nextStep.StepOrder;

            var starterUser = await _context.Users.FindAsync(process.CreatedByUserId);
            var assignedRoleId = ResolveRoleId(nextStep, starterUser ?? user);

            _context.WorkItems.Add(new WorkItem
            {
                ProcessInstanceId = process.Id,
                WorkflowStepId = nextStep.Id,
                AssignedToRoleId = assignedRoleId,
                Status = "Beklemede"
            });
        }

        await _context.SaveChangesAsync();

        return await GetProcessDetailAsync(process.Id);
    }

    public async Task<ProcessDetailDto?> RejectStepAsync(int workItemId, int userId, string? notes)
    {
        var workItem = await _context.WorkItems
            .Include(w => w.Process)
            .Include(w => w.WorkflowStep)
            .FirstOrDefaultAsync(w => w.Id == workItemId);

        if (workItem == null || workItem.Status != "Beklemede")
            return null;

        var process = workItem.Process;

        workItem.Status = "Reddedildi";
        workItem.CompletedAt = DateTime.UtcNow;
        workItem.CompletedByUserId = userId;
        workItem.Notes = notes;

        process.Status = "Reddedildi";
        process.CompletedAt = DateTime.UtcNow;

        _context.ProcessHistories.Add(new ProcessHistory
        {
            ProcessInstanceId = process.Id,
            Action = $"{workItem.WorkflowStep.Name} — Reddedildi",
            Comment = notes,
            PerformedByUserId = userId
        });

        await _context.SaveChangesAsync();

        return await GetProcessDetailAsync(process.Id);
    }

    public async Task<ProcessDetailDto?> GetProcessDetailAsync(int processId)
    {
        return await _context.Processes
            .Where(p => p.Id == processId)
            .Select(p => new ProcessDetailDto
            {
                Id = p.Id,
                WorkflowName = p.WorkflowDefinition.Name,
                WorkflowCode = p.WorkflowDefinition.Code,
                Category = p.WorkflowDefinition.Category,
                Status = p.Status,
                CurrentStepOrder = p.CurrentStepOrder,
                DataJson = p.DataJson,
                CreatedByName = p.CreatedByUser.DisplayName,
                CreatedAt = p.CreatedAt,
                CompletedAt = p.CompletedAt,
                WorkItems = p.WorkItems
                    .OrderBy(w => w.WorkflowStep.StepOrder)
                    .Select(w => new WorkItemDto
                    {
                        Id = w.Id,
                        ProcessInstanceId = p.Id,
                        ProcessName = p.WorkflowDefinition.Name,
                        StepName = w.WorkflowStep.Name,
                        StepOrder = w.WorkflowStep.StepOrder,
                        ActionType = w.WorkflowStep.ActionType,
                        Status = w.Status,
                        AssignedToRoleName = w.AssignedToRole.Name,
                        AssignedToUserName = w.AssignedToUser != null ? w.AssignedToUser.DisplayName : null,
                        Notes = w.Notes,
                        CreatedAt = w.CreatedAt,
                        CompletedAt = w.CompletedAt,
                        CompletedByName = w.CompletedByUser != null ? w.CompletedByUser.DisplayName : null,
                        CreatedByName = p.CreatedByUser.DisplayName
                    }).ToList(),
                History = p.History
                    .OrderByDescending(h => h.PerformedAt)
                    .Select(h => new HistoryDto
                    {
                        Action = h.Action,
                        Comment = h.Comment,
                        PerformedByName = h.PerformedByUser.DisplayName,
                        PerformedAt = h.PerformedAt
                    }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ProcessDto>> GetProcessesAsync(int? userId = null, string? status = null)
    {
        var query = _context.Processes.AsQueryable();

        if (userId.HasValue)
            query = query.Where(p => p.CreatedByUserId == userId.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProcessDto
            {
                Id = p.Id,
                WorkflowName = p.WorkflowDefinition.Name,
                WorkflowCode = p.WorkflowDefinition.Code,
                Status = p.Status,
                CurrentStepOrder = p.CurrentStepOrder,
                CurrentStepName = p.WorkflowDefinition.Steps
                    .Where(s => s.StepOrder == p.CurrentStepOrder)
                    .Select(s => s.Name)
                    .FirstOrDefault(),
                CreatedByName = p.CreatedByUser.DisplayName,
                CreatedAt = p.CreatedAt,
                CompletedAt = p.CompletedAt
            })
            .ToListAsync();
    }

    public async Task<List<WorkItemDto>> GetWorkItemsAsync(int roleId, int? userId = null, string? status = null)
    {
        var query = _context.WorkItems
            .Where(w => w.AssignedToRoleId == roleId ||
                        (w.AssignedToUserId != null && w.AssignedToUserId == userId));

        if (!string.IsNullOrEmpty(status))
            query = query.Where(w => w.Status == status);

        return await query
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new WorkItemDto
            {
                Id = w.Id,
                ProcessInstanceId = w.ProcessInstanceId,
                ProcessName = w.Process.WorkflowDefinition.Name,
                StepName = w.WorkflowStep.Name,
                StepOrder = w.WorkflowStep.StepOrder,
                ActionType = w.WorkflowStep.ActionType,
                Status = w.Status,
                AssignedToRoleName = w.AssignedToRole.Name,
                AssignedToUserName = w.AssignedToUser != null ? w.AssignedToUser.DisplayName : null,
                Notes = w.Notes,
                CreatedAt = w.CreatedAt,
                CompletedAt = w.CompletedAt,
                CompletedByName = w.CompletedByUser != null ? w.CompletedByUser.DisplayName : null,
                CreatedByName = w.Process.CreatedByUser.DisplayName
            })
            .ToListAsync();
    }

    private WorkflowStep? FindNextStep(IEnumerable<WorkflowStep> steps, int currentOrder, string formDataJson)
    {
        var orderedSteps = steps
            .Where(s => s.StepOrder > currentOrder)
            .OrderBy(s => s.StepOrder);

        foreach (var step in orderedSteps)
        {
            if (!step.IsConditional || string.IsNullOrEmpty(step.ConditionJson))
                return step;

            if (EvaluateCondition(step.ConditionJson, formDataJson))
                return step;
        }

        return null;
    }

    private static bool EvaluateCondition(string conditionJson, string formDataJson)
    {
        try
        {
            using var condDoc = JsonDocument.Parse(conditionJson);
            using var dataDoc = JsonDocument.Parse(formDataJson);

            var cond = condDoc.RootElement;
            var field = cond.GetProperty("field").GetString()!;
            var op = cond.GetProperty("operator").GetString()!;
            var condValue = cond.GetProperty("value");

            if (!dataDoc.RootElement.TryGetProperty(field, out var dataValue))
                return false;

            if (condValue.ValueKind == JsonValueKind.Number && dataValue.ValueKind == JsonValueKind.Number)
            {
                var a = dataValue.GetDouble();
                var b = condValue.GetDouble();

                return op switch
                {
                    ">=" => a >= b,
                    "<=" => a <= b,
                    ">" => a > b,
                    "<" => a < b,
                    "==" => Math.Abs(a - b) < 0.001,
                    "!=" => Math.Abs(a - b) >= 0.001,
                    _ => false
                };
            }

            var aStr = dataValue.ToString();
            var bStr = condValue.ToString();

            return op switch
            {
                "==" => aStr == bStr,
                "!=" => aStr != bStr,
                _ => false
            };
        }
        catch
        {
            return false;
        }
    }

    private int ResolveRoleId(WorkflowStep step, User starterUser)
    {
        if (string.IsNullOrEmpty(step.AssignmentRule))
            return step.AssignedRoleId;

        if (step.AssignmentRule == "DEPT_MANAGER")
        {
            var managerRole = _context.Roles
                .FirstOrDefault(r => r.DepartmentId == starterUser.DepartmentId && r.Level == 2);

            return managerRole?.Id ?? step.AssignedRoleId;
        }

        return step.AssignedRoleId;
    }
}

using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;

namespace WizardFormApi.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardAsync(int userId, int roleId, int roleLevel, int departmentId)
    {
        var processQuery = ApplyHierarchyFilter(
            _context.Processes.AsQueryable(),
            userId, roleLevel, departmentId);

        var stats = new DashboardStatsDto
        {
            TotalProcesses = await processQuery.CountAsync(),
            ActiveProcesses = await processQuery.CountAsync(p => p.Status == "Aktif"),
            CompletedProcesses = await processQuery.CountAsync(p => p.Status == "Tamamlandı"),
            RejectedProcesses = await processQuery.CountAsync(p => p.Status == "Reddedildi"),
            PendingWorkItems = await _context.WorkItems
                .CountAsync(w => w.Status == "Beklemede" &&
                    (w.AssignedToRoleId == roleId ||
                     (w.AssignedToUserId != null && w.AssignedToUserId == userId)))
        };

        var recentProcesses = await processQuery
            .OrderByDescending(p => p.CreatedAt)
            .Take(10)
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

        var pendingWorkItems = await _context.WorkItems
            .Where(w => w.Status == "Beklemede" &&
                (w.AssignedToRoleId == roleId ||
                 (w.AssignedToUserId != null && w.AssignedToUserId == userId)))
            .OrderByDescending(w => w.CreatedAt)
            .Take(10)
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

        return new DashboardDto
        {
            Stats = stats,
            RecentProcesses = recentProcesses,
            PendingWorkItems = pendingWorkItems
        };
    }

    private static IQueryable<Models.ProcessInstance> ApplyHierarchyFilter(
        IQueryable<Models.ProcessInstance> query,
        int userId, int roleLevel, int departmentId)
    {
        return roleLevel switch
        {
            1 => query,
            2 => query.Where(p => p.CreatedByUser.DepartmentId == departmentId),
            3 => query.Where(p => p.CreatedByUser.DepartmentId == departmentId),
            _ => query.Where(p => p.CreatedByUserId == userId)
        };
    }
}

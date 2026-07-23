namespace WizardFormApi.DTOs;

public class DashboardDto
{
    public DashboardStatsDto Stats { get; set; } = new();
    public List<ProcessDto> RecentProcesses { get; set; } = new();
    public List<WorkItemDto> PendingWorkItems { get; set; } = new();
}

public class DashboardStatsDto
{
    public int TotalProcesses { get; set; }
    public int ActiveProcesses { get; set; }
    public int CompletedProcesses { get; set; }
    public int RejectedProcesses { get; set; }
    public int PendingWorkItems { get; set; }
}

namespace WizardFormApi.Models;

public class WorkItem
{
    public int Id { get; set; }
    public int ProcessInstanceId { get; set; }
    public int WorkflowStepId { get; set; }
    public int? AssignedToUserId { get; set; }
    public int AssignedToRoleId { get; set; }
    public string Status { get; set; } = "Beklemede";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int? CompletedByUserId { get; set; }

    public ProcessInstance Process { get; set; } = null!;
    public WorkflowStep WorkflowStep { get; set; } = null!;
    public User? AssignedToUser { get; set; }
    public Role AssignedToRole { get; set; } = null!;
    public User? CompletedByUser { get; set; }
}

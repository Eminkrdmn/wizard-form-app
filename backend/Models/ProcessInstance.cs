namespace WizardFormApi.Models;

public class ProcessInstance
{
    public int Id { get; set; }
    public int WorkflowDefinitionId { get; set; }
    public int? FormId { get; set; }
    public string DataJson { get; set; } = "{}";
    public string Status { get; set; } = "Aktif";
    public int CurrentStepOrder { get; set; } = 1;
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int? ParentProcessId { get; set; }

    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
    public ProcessInstance? ParentProcess { get; set; }
    public List<ProcessInstance> ChildProcesses { get; set; } = new();
    public FormDefinition? Form { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public List<WorkItem> WorkItems { get; set; } = new();
    public List<ProcessHistory> History { get; set; } = new();
}

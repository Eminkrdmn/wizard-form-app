namespace WizardFormApi.Models;

public class WorkflowDefinition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPredefined { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public int? FormTemplateId { get; set; }

    public FormDefinition? FormTemplate { get; set; }
    public List<WorkflowStep> Steps { get; set; } = new();
    public List<ProcessInstance> Instances { get; set; } = new();
}

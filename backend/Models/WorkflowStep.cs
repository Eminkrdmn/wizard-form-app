namespace WizardFormApi.Models;

public class WorkflowStep
{
    public int Id { get; set; }
    public int WorkflowDefinitionId { get; set; }
    public int StepOrder { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AssignedRoleId { get; set; }
    public string ActionType { get; set; } = "Approve";
    public string? AssignmentRule { get; set; }
    public bool IsConditional { get; set; }
    public string? ConditionJson { get; set; }

    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
    public Role AssignedRole { get; set; } = null!;
}

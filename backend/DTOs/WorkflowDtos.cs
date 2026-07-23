namespace WizardFormApi.DTOs;

public class StartProcessRequest
{
    public int WorkflowDefinitionId { get; set; }
    public string FormDataJson { get; set; } = "{}";
}

public class ActionRequest
{
    public string? Notes { get; set; }
}

public class ProcessDto
{
    public int Id { get; set; }
    public string WorkflowName { get; set; } = string.Empty;
    public string WorkflowCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int CurrentStepOrder { get; set; }
    public string? CurrentStepName { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class WorkItemDto
{
    public int Id { get; set; }
    public int ProcessInstanceId { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public string StepName { get; set; } = string.Empty;
    public int StepOrder { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string AssignedToRoleName { get; set; } = string.Empty;
    public string? AssignedToUserName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? CompletedByName { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
}

public class ProcessDetailDto
{
    public int Id { get; set; }
    public string WorkflowName { get; set; } = string.Empty;
    public string WorkflowCode { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int CurrentStepOrder { get; set; }
    public string DataJson { get; set; } = "{}";
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<WorkItemDto> WorkItems { get; set; } = new();
    public List<HistoryDto> History { get; set; } = new();
}

public class HistoryDto
{
    public string Action { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public string PerformedByName { get; set; } = string.Empty;
    public DateTime PerformedAt { get; set; }
}

public class WorkflowListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPredefined { get; set; }
    public int StepCount { get; set; }
    public int? FormTemplateId { get; set; }
}

public class WorkflowDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPredefined { get; set; }
    public int? FormTemplateId { get; set; }
    public string? FormFieldsJson { get; set; }
    public List<WorkflowStepDto> Steps { get; set; } = new();
}

public class WorkflowStepDto
{
    public int Id { get; set; }
    public int StepOrder { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string AssignedRoleName { get; set; } = string.Empty;
    public bool IsConditional { get; set; }
    public string? ConditionJson { get; set; }
    public string? AssignmentRule { get; set; }
}

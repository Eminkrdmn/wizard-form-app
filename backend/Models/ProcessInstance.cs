namespace WizardFormApi.Models;

public class ProcessInstance
{
    public int Id { get; set; }
    public int FormId { get; set; }
    public string FormName { get; set; } = string.Empty;
    public string DataJson { get; set; } = "{}";
    public string Status { get; set; } = "Beklemede";
    public string AssignedRole { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public FormDefinition? Form { get; set; }
    public List<ProcessHistory> History { get; set; } = new();
}
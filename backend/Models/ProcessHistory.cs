namespace WizardFormApi.Models;

public class ProcessHistory
{
    public int Id { get; set; }
    public int ProcessInstanceId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    public ProcessInstance? Process { get; set; }
}
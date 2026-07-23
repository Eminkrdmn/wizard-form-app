namespace WizardFormApi.Models;

public class ProcessHistory
{
    public int Id { get; set; }
    public int ProcessInstanceId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public int PerformedByUserId { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    public ProcessInstance Process { get; set; } = null!;
    public User PerformedByUser { get; set; } = null!;
}

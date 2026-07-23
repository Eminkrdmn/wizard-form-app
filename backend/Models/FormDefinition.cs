namespace WizardFormApi.Models;

public class FormDefinition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FieldsJson { get; set; } = "[]";
    public bool IsSystemForm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }
}

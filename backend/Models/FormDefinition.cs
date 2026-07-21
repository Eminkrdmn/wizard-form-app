namespace WizardFormApi.Models;

public class FormDefinition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FieldsJson { get; set; } = "[]";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
}
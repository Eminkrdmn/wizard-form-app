namespace WizardFormApi.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public List<Role> Roles { get; set; } = new();
}

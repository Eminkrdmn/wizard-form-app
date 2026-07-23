namespace WizardFormApi.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int Level { get; set; }

    public Department Department { get; set; } = null!;
    public List<User> Users { get; set; } = new();
}

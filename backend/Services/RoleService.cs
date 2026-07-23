using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;

namespace WizardFormApi.Services;

public class RoleService
{
    private readonly AppDbContext _context;

    public RoleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleDto>> GetAllAsync(int? departmentId = null)
    {
        var query = _context.Roles.AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(r => r.DepartmentId == departmentId.Value);

        return await query
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Code = r.Code,
                Level = r.Level,
                DepartmentName = r.Department.Name,
                DepartmentCode = r.Department.Code,
                UserCount = r.Users.Count
            })
            .OrderBy(r => r.DepartmentName)
            .ThenBy(r => r.Level)
            .ToListAsync();
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .Where(r => r.Id == id)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Code = r.Code,
                Level = r.Level,
                DepartmentName = r.Department.Name,
                DepartmentCode = r.Department.Code,
                UserCount = r.Users.Count
            })
            .FirstOrDefaultAsync();
    }
}

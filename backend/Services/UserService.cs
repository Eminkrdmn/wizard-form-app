using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;

namespace WizardFormApi.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                DisplayName = u.DisplayName,
                Email = u.Email,
                IsActive = u.IsActive,
                RoleName = u.Role.Name,
                RoleCode = u.Role.Code,
                RoleLevel = u.Role.Level,
                DepartmentName = u.Department.Name,
                DepartmentCode = u.Department.Code
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<UserDto>> GetAllAsync(int? departmentId = null, int? roleLevel = null)
    {
        var query = _context.Users.AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(u => u.DepartmentId == departmentId.Value);

        if (roleLevel.HasValue)
            query = query.Where(u => u.Role.Level == roleLevel.Value);

        return await query
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                DisplayName = u.DisplayName,
                Email = u.Email,
                IsActive = u.IsActive,
                RoleName = u.Role.Name,
                RoleCode = u.Role.Code,
                RoleLevel = u.Role.Level,
                DepartmentName = u.Department.Name,
                DepartmentCode = u.Department.Code
            })
            .OrderBy(u => u.DepartmentName)
            .ThenBy(u => u.RoleLevel)
            .ThenBy(u => u.DisplayName)
            .ToListAsync();
    }
}

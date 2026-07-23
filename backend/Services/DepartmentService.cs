using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;

namespace WizardFormApi.Services;

public class DepartmentService
{
    private readonly AppDbContext _context;

    public DepartmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        return await _context.Departments
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                RoleCount = d.Roles.Count,
                UserCount = d.Roles.SelectMany(r => r.Users).Count()
            })
            .ToListAsync();
    }

    public async Task<DepartmentDetailDto?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Where(d => d.Id == id)
            .Select(d => new DepartmentDetailDto
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                Roles = d.Roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code,
                    Level = r.Level,
                    DepartmentName = d.Name,
                    DepartmentCode = d.Code,
                    UserCount = r.Users.Count
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
}

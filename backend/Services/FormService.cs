using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;
using WizardFormApi.DTOs;
using WizardFormApi.Models;

namespace WizardFormApi.Services;

public class FormService
{
    private readonly AppDbContext _context;

    public FormService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FormListDto>> GetAllAsync(bool? isSystemForm = null)
    {
        var query = _context.Forms.AsQueryable();

        if (isSystemForm.HasValue)
            query = query.Where(f => f.IsSystemForm == isSystemForm.Value);

        return await query
            .Select(f => new FormListDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                IsSystemForm = f.IsSystemForm,
                CreatedAt = f.CreatedAt,
                CreatedByName = f.CreatedByUser != null ? f.CreatedByUser.DisplayName : null
            })
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<FormDetailDto?> GetByIdAsync(int id)
    {
        return await _context.Forms
            .Where(f => f.Id == id)
            .Select(f => new FormDetailDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                FieldsJson = f.FieldsJson,
                IsSystemForm = f.IsSystemForm,
                CreatedAt = f.CreatedAt,
                CreatedByName = f.CreatedByUser != null ? f.CreatedByUser.DisplayName : null
            })
            .FirstOrDefaultAsync();
    }

    public async Task<FormDetailDto> CreateAsync(CreateFormRequest request, int userId)
    {
        var form = new FormDefinition
        {
            Name = request.Name,
            Description = request.Description,
            FieldsJson = request.FieldsJson,
            IsSystemForm = false,
            CreatedByUserId = userId
        };

        _context.Forms.Add(form);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(form.Id))!;
    }

    public async Task<FormDetailDto?> UpdateAsync(int id, UpdateFormRequest request)
    {
        var form = await _context.Forms.FindAsync(id);
        if (form == null)
            return null;

        if (form.IsSystemForm)
            return null;

        form.Name = request.Name;
        form.Description = request.Description;
        form.FieldsJson = request.FieldsJson;

        await _context.SaveChangesAsync();

        return (await GetByIdAsync(form.Id))!;
    }
}

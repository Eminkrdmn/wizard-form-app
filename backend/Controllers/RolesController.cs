using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WizardFormApi.Services;

namespace WizardFormApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly RoleService _service;

    public RolesController(RoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? departmentId)
    {
        var roles = await _service.GetAllAsync(departmentId);
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await _service.GetByIdAsync(id);
        if (role == null)
            return NotFound(new { message = "Rol bulunamadı" });
        return Ok(role);
    }
}

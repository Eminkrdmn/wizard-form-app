using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WizardFormApi.Services;

namespace WizardFormApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(DashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleIdClaim = User.FindFirstValue("role_id");
        var roleLevelClaim = User.FindFirstValue("role_level");
        var deptIdClaim = User.FindFirstValue("department_id");

        if (userIdClaim == null || roleIdClaim == null ||
            roleLevelClaim == null || deptIdClaim == null ||
            !int.TryParse(userIdClaim, out var userId) ||
            !int.TryParse(roleIdClaim, out var roleId) ||
            !int.TryParse(roleLevelClaim, out var roleLevel) ||
            !int.TryParse(deptIdClaim, out var deptId))
            return Unauthorized(new { message = "Geçersiz token" });

        var dashboard = await _service.GetDashboardAsync(userId, roleId, roleLevel, deptId);
        return Ok(dashboard);
    }
}

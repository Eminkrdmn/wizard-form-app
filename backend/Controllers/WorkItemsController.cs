using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WizardFormApi.DTOs;
using WizardFormApi.Services;

namespace WizardFormApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkItemsController : ControllerBase
{
    private readonly WorkflowEngine _engine;

    public WorkItemsController(WorkflowEngine engine)
    {
        _engine = engine;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyItems([FromQuery] string? status)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleIdClaim = User.FindFirstValue("role_id");

        if (userIdClaim == null || roleIdClaim == null ||
            !int.TryParse(userIdClaim, out var userId) ||
            !int.TryParse(roleIdClaim, out var roleId))
            return Unauthorized(new { message = "Geçersiz token" });

        var items = await _engine.GetWorkItemsAsync(roleId, userId, status);
        return Ok(items);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] ActionRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Geçersiz token" });

        var result = await _engine.ApproveStepAsync(id, userId, request.Notes);
        if (result == null)
            return BadRequest(new { message = "İş kalemi bulunamadı veya zaten işlenmiş" });

        return Ok(result);
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(int id, [FromBody] ActionRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Geçersiz token" });

        var result = await _engine.RejectStepAsync(id, userId, request.Notes);
        if (result == null)
            return BadRequest(new { message = "İş kalemi bulunamadı veya zaten işlenmiş" });

        return Ok(result);
    }
}

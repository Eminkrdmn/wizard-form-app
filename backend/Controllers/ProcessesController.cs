using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WizardFormApi.DTOs;
using WizardFormApi.Services;

namespace WizardFormApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessesController : ControllerBase
{
    private readonly WorkflowEngine _engine;

    public ProcessesController(WorkflowEngine engine)
    {
        _engine = engine;
    }

    [HttpPost]
    public async Task<IActionResult> Start([FromBody] StartProcessRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Geçersiz token" });

        var result = await _engine.StartProcessAsync(request, userId);
        if (result == null)
            return BadRequest(new { message = "Süreç başlatılamadı. Workflow bulunamadı veya aktif değil." });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] bool? mine)
    {
        int? userId = null;
        if (mine == true)
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim, out var id))
                userId = id;
        }

        var processes = await _engine.GetProcessesAsync(userId, status);
        return Ok(processes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var process = await _engine.GetProcessDetailAsync(id);
        if (process == null)
            return NotFound(new { message = "Süreç bulunamadı" });
        return Ok(process);
    }
}

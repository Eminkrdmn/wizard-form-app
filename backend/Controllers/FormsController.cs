using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WizardFormApi.DTOs;
using WizardFormApi.Services;

namespace WizardFormApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormsController : ControllerBase
{
    private readonly FormService _service;

    public FormsController(FormService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? isSystemForm)
    {
        var forms = await _service.GetAllAsync(isSystemForm);
        return Ok(forms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var form = await _service.GetByIdAsync(id);
        if (form == null)
            return NotFound(new { message = "Form bulunamadı" });
        return Ok(form);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFormRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Geçersiz token" });

        var form = await _service.CreateAsync(request, userId);
        return CreatedAtAction(nameof(GetById), new { id = form.Id }, form);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFormRequest request)
    {
        var form = await _service.UpdateAsync(id, request);
        if (form == null)
            return BadRequest(new { message = "Form bulunamadı veya sistem formu düzenlenemez" });
        return Ok(form);
    }
}

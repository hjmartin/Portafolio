using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Api.Security;
using Portafolio.Application.Dtos;
using Portafolio.Application.Interfaces.Services;

namespace Portafolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.PortfolioRead)]
public sealed class TechnologiesController : ControllerBase
{
    private readonly ITechnologyService _service;

    public TechnologiesController(ITechnologyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TechnologyDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TechnologyDto>> GetById([FromRoute] Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<ActionResult<TechnologyDto>> Create([FromBody] CreateTechnologyDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<ActionResult<TechnologyDto>> Update([FromRoute] Guid id, [FromBody] UpdateTechnologyDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}

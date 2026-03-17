using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Api.Security;
using Portafolio.Application.Dtos;
using Portafolio.Application.Interfaces.Services;

namespace Portafolio.Api.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicies.PortfolioRead)]
public sealed class PortfolioProjectsController : ControllerBase
{
    private readonly IPortfolioProjectService _service;

    public PortfolioProjectsController(IPortfolioProjectService service)
    {
        _service = service;
    }

    [HttpGet("api/portfolioprofiles/{profileId:guid}/projects")]
    public async Task<ActionResult<IReadOnlyList<PortfolioProjectDto>>> GetByProfile([FromRoute] Guid profileId)
        => Ok(await _service.GetByProfileAsync(profileId));

    [HttpPost("api/portfolioprofiles/{profileId:guid}/projects")]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<ActionResult<PortfolioProjectDto>> Create([FromRoute] Guid profileId, [FromBody] CreatePortfolioProjectDto dto)
    {
        var created = await _service.CreateAsync(profileId, dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("api/portfolio-projects/{id:guid}")]
    public async Task<ActionResult<PortfolioProjectDto>> GetById([FromRoute] Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPut("api/portfolio-projects/{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<ActionResult<PortfolioProjectDto>> Update([FromRoute] Guid id, [FromBody] UpdatePortfolioProjectDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("api/portfolio-projects/{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("api/portfolio-projects/{id:guid}/technologies/{technologyId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<IActionResult> AddTechnology([FromRoute] Guid id, [FromRoute] Guid technologyId)
    {
        await _service.AddTechnologyAsync(id, technologyId);
        return NoContent();
    }

    [HttpDelete("api/portfolio-projects/{id:guid}/technologies/{technologyId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
    public async Task<IActionResult> RemoveTechnology([FromRoute] Guid id, [FromRoute] Guid technologyId)
    {
        await _service.RemoveTechnologyAsync(id, technologyId);
        return NoContent();
    }
}


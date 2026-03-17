using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Api.Security;
using Portafolio.Application.Dtos;
using Portafolio.Application.Interfaces.Services;

namespace Portafolio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AuthorizationPolicies.PortfolioRead)]
    public class PortfolioProfilesController : ControllerBase
    {
        private readonly IPortfolioProfileService _service;

        public PortfolioProfilesController(IPortfolioProfileService service)
            => _service = service;

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
        public async Task<ActionResult<PortfolioProfileDto>> Create([FromBody] CreatePortfolioProfileDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PortfolioProfileDto>> GetById([FromRoute] Guid id)
            => Ok(await _service.GetByIdAsync(id)); // el service se encarga de 404

        [HttpPut("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.PortfolioWrite)]
        public async Task<ActionResult<PortfolioProfileDto>> Update([FromRoute] Guid id, [FromBody] UpdatePortfolioProfileDto dto)
            => Ok(await _service.UpdateAsync(id, dto));
    }
}

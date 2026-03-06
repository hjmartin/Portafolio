using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portafolio.Application.Common.Exceptions;
using Portafolio.Application.Dtos;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Services;

public class PortfolioProfileService : IPortfolioProfileService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PortfolioProfileService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PortfolioProfileDto> CreateAsync(CreatePortfolioProfileDto dto)
    {
    
        var entity = _mapper.Map<PortfolioProfile>(dto);

        await _uow.Repo<PortfolioProfile>().AddAsync(entity);
        await _uow.SaveAsync();

        return _mapper.Map<PortfolioProfileDto>(entity);
    }
    public async Task<PortfolioProfileDto?> GetByIdAsync(Guid id)
    {
        // Necesitas Query() en tu repo genérico para poder usar Include
        var entity = await _uow.Repo<PortfolioProfile>()
            .Query()
            .Include(x => x.Projects)
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ? null : _mapper.Map<PortfolioProfileDto>(entity);
    }

    public async Task<PortfolioProfileDto> UpdateAsync(Guid id, UpdatePortfolioProfileDto dto)
    {
        var repo = _uow.Repo<PortfolioProfile>();

        var entity = await repo.Query()
            .Include(x => x.Projects)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null) throw new KeyNotFoundException("Profile no existe");

        // Mapea encima del entity existente (EF tracking)
        _mapper.Map(dto, entity);

        repo.Update(entity);
        await _uow.SaveAsync();

        return _mapper.Map<PortfolioProfileDto>(entity);
    }
}

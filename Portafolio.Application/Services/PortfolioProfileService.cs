using AutoMapper;
using Portafolio.Application.Common.Exceptions;
using Portafolio.Application.Dtos;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Application.Interfaces.Services;

namespace Portafolio.Application.Services;

public class PortfolioProfileService : IPortfolioProfileService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IPortfolioProfileRepository _profiles;

    public PortfolioProfileService(
        IUnitOfWork uow,
        IMapper mapper,
        IPortfolioProfileRepository profiles)
    {
        _uow = uow;
        _mapper = mapper;
        _profiles = profiles;
    }

    public async Task<PortfolioProfileDto> CreateAsync(CreatePortfolioProfileDto dto)
    {
        var entity = PortfolioProfile.Create(
            dto.FullName,
            dto.Headline,
            dto.Summary,
            dto.Location,
            dto.PublicEmail);

        await _profiles.AddAsync(entity);
        await _uow.SaveAsync();

        return _mapper.Map<PortfolioProfileDto>(entity);
    }

    public async Task<PortfolioProfileDto> GetByIdAsync(Guid id)
    {
        var entity = await _profiles.GetByIdWithProjectsAsync(id, asNoTracking: true);

        if (entity is null)
            throw new NotFoundException("Profile no existe", "portfolio_profile_not_found");

        return _mapper.Map<PortfolioProfileDto>(entity);
    }

    public async Task<PortfolioProfileDto> UpdateAsync(Guid id, UpdatePortfolioProfileDto dto)
    {
        var entity = await _profiles.GetByIdForUpdateAsync(id);

        if (entity is null)
            throw new NotFoundException("Profile no existe", "portfolio_profile_not_found");

        entity.UpdateDetails(
            dto.FullName,
            dto.Headline,
            dto.Summary,
            dto.Location,
            dto.PublicEmail,
            dto.PhotoUrl,
            dto.ResumeUrl);

        _profiles.Update(entity);
        await _uow.SaveAsync();

        return _mapper.Map<PortfolioProfileDto>(entity);
    }
}

using AutoMapper;
using Portafolio.Application.Dtos;

namespace Portafolio.Application.Mapping;

public class PortfolioMappingProfile : Profile
{
    public PortfolioMappingProfile()
    {
        CreateMap<PortfolioProfile, PortfolioProfileDto>();

        CreateMap<PortfolioProject, PortfolioProjectDto>()
            .ForMember(d => d.Technologies, opt =>
                opt.MapFrom(s => s.Technologies.Select(t => t.Technology)));

        CreateMap<Technology, TechnologyDto>();
    }
}

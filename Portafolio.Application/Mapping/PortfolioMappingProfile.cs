using AutoMapper;
using Portafolio.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Mapping;

public class PortfolioMappingProfile : Profile
{
    public PortfolioMappingProfile()
    {
        // Entity -> DTO
        CreateMap<PortfolioProfile, PortfolioProfileDto>();
        CreateMap<PortfolioProject, PortfolioProjectDto>();

        // DTO -> Entity (Create)
        CreateMap<CreatePortfolioProfileDto, PortfolioProfile>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Projects, opt => opt.Ignore()); // se manejan aparte si aplica

        // DTO -> Entity (Update)
        CreateMap<UpdatePortfolioProfileDto, PortfolioProfile>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Projects, opt => opt.Ignore());

        // Si en algún momento haces PATCH y quieres ignorar nulls:
        // CreateMap<UpdatePortfolioProfileDto, PortfolioProfile>()
        //   .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}

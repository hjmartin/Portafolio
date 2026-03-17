using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Application.Interfaces.Security;
using Portafolio.Infrastructure.Persistence;
using Portafolio.Infrastructure.Persistence.Interceptors;
using Portafolio.Infrastructure.Repositories;
using Portafolio.Infrastructure.Security;

namespace Portafolio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<AuditSaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPortfolioProfileRepository, PortfolioProfileRepository>();
        services.AddScoped<IPortfolioProjectRepository, PortfolioProjectRepository>();
        services.AddScoped<ITechnologyRepository, TechnologyRepository>();

        return services;
    }
}

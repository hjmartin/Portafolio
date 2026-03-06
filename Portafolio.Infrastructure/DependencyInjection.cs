using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Application.Interfaces.Security;
using Portafolio.Infrastructure.Persistence;
using Portafolio.Infrastructure.Persistence.Interceptors;
using Portafolio.Infrastructure.Repositories;
using Portafolio.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Portafolio.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registra el HttpContextAccessor para acceder al contexto HTTP en servicios y obtener información del usuario actual
        services.AddHttpContextAccessor();
        // Registra el servicio de CurrentUser que implementa ICurrentUser, para obtener datos del usuario autenticado en el contexto HTTP.
        services.AddScoped<ICurrentUser, CurrentUser>();
        // Registra el interceptor de auditoría para que se ejecute automáticamente en cada operación de guardado (SaveChanges) del DbContext.
        services.AddScoped<AuditSaveChangesInterceptor>();

        // Configura el DbContext con SQL Server usando la cadena de conexión del appsettings.json
        // Configura el DbContext con SQL Server usando la cadena de conexión del appsettings.json
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });

        // Registra el UnitOfWork y los repositorios genéricos
        services.AddScoped<IUnitOfWork, UnitOfWork>();



        return services;
    }
}

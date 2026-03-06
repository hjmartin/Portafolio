using Microsoft.Extensions.DependencyInjection;
using Portafolio.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //- Registra todas las clases Services dentro del namespace MerchantService.Application.Services.
        //- Cada clase se registra contra sus interfaces implementadas
        //- Convención: los Services representan casos de uso y deben terminar en "Service"
        //- Reemplaza el services.AddScoped<IPruebaService, PruebaService>();
        services.Scan(scan => scan
            .FromAssemblyOf<ApplicationAssemblyMarker>()
            .AddClasses(c => c.InNamespaces("Portafolio.Application.Services")
                                  .Where(t => t.Name.EndsWith("Service") && !t.IsAbstract))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        //- Escanea el assembly de Application y carga automáticamente todos los Profiles de automapper.
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ApplicationAssemblyMarker).Assembly);
        });

        //- Registra el ServiceContext para inyección de dependencias
        services.AddScoped<ServiceContext>();

        return services;


    }
}

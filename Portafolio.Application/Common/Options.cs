using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Common;

public sealed class Options
{

    //aqui podemos  incluir cualquier configuración relacionada con PayWCC, como la URL de la API, claves de acceso, etc.
    // Ejemplo: un link de para subir imagenes
    public string ApiUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}

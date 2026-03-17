using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portafolio.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Common;

public sealed class ServiceContext
{
    public IUnitOfWork Uow { get; }
    public IMapper Mapper { get; }
    public ILoggerFactory LoggerFactory { get; }
    public IOptions<Options> Options { get; }

    public ServiceContext(
        IUnitOfWork uow,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IOptions<Options> options)
    {
        Uow = uow;
        Mapper = mapper;
        LoggerFactory = loggerFactory;
        Options = options;
    }
}

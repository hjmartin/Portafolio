using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Application.Common.Exceptions;
using Portafolio.Domain.Common;

namespace Portafolio.Api.Middleware;

public sealed class ApiExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;

    public ApiExceptionHandlingMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
    {
        ProblemDetails problem;

        switch (ex)
        {
            case AppException appEx:
                context.Response.StatusCode = appEx.StatusCode;
                problem = new ProblemDetails
                {
                    Status = appEx.StatusCode,
                    Title = appEx.Code,
                    Detail = appEx.Message,
                    Type = $"https://httpstatuses.com/{appEx.StatusCode}"
                };
                break;

            case DomainRuleException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "domain_rule_violation",
                    Detail = ex.Message,
                    Type = "https://httpstatuses.com/400"
                };
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "unauthorized",
                    Detail = ex.Message,
                    Type = "https://httpstatuses.com/401"
                };
                break;

            default:
                logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "internal_server_error",
                    Detail = "An unexpected error occurred.",
                    Type = "https://httpstatuses.com/500"
                };
                break;
        }

        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}

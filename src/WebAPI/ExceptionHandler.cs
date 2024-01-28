using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

public sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is null)
            return false;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = httpContext.Response.StatusCode,
            Detail = exceptionHandlerPathFeature.Error.Message,
            Instance = exceptionHandlerPathFeature.Path
        }), cancellationToken);
        return true;
    }
}

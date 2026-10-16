namespace overload.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var baseResult = exception switch
        {
            ApiBaseException baseEx => (int)baseEx.code,
            _ => 500
        };
        httpContext.Response.StatusCode = baseResult;
        var problem = new ProblemDetails
        {
            Status = baseResult,
            Title = "A error happen when try run the request",
            Detail = exception.Message
        };
        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken
        );
        return true;
    }
}
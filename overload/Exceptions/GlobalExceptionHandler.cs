namespace overload.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var baseResult = exception switch
        {
            ApiBaseException<string> baseEx => (int)baseEx.code,
            _ => 500
        };
        httpContext.Response.StatusCode = baseResult;
        var problem = new ProblemDetails
        {
            Status = baseResult,
            Title = "An error occurred while processing your request",
            Detail = exception.Message
        };
        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken
        );
        return true;
    }
}
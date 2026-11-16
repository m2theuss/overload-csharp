using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using overload.Services.Shared;

namespace overload.Extensions;

public static class ResultExtension
{
    public static IActionResult ToActionResult<T> (this Result<T> result, HttpContext httpContext)
    {
        if (result.IsOk)
        {
            if(result.Data is not null) return new ObjectResult(result.Data);
            if(!string.IsNullOrWhiteSpace(result.Detail)) return new ObjectResult(new {message = result.Detail});
            return new OkResult();
        }

        int statusCode = ToIntStatusCode(result.Code);

        var problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var problemDetails = problemDetailsFactory.CreateProblemDetails
        (
            httpContext: httpContext,
            title: result.Title, 
            detail: result.Detail, 
            statusCode: statusCode
        );
        return new ObjectResult(problemDetails){StatusCode = statusCode};
    }
    private static int ToIntStatusCode(ErrorCode code)
    {
        return code switch
        {
            ErrorCode.Ok => StatusCodes.Status200OK,

            ErrorCode.InvalidData => StatusCodes.Status400BadRequest,

            ErrorCode.BadCredentials or
            ErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,

            ErrorCode.NotFound => StatusCodes.Status404NotFound,

            ErrorCode.UsernameAlreadyExists or
            ErrorCode.EmailAlreadyExists or
            ErrorCode.UsernameOrEmailAlreadyExist or
            ErrorCode.NameConflict or
            ErrorCode.DataConflict => StatusCodes.Status409Conflict,

            ErrorCode.InternalError or
            ErrorCode.DatabaseSaveFailure or
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
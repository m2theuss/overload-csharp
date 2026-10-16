using System.Net;
using overload.Services.Shared;
namespace overload.Exceptions;

public abstract class ApiBaseException<T> : Exception
{
    public HttpStatusCode code{get;}
    public ApiErrorResponse<T>? ErrorResponse{get;}
    protected ApiBaseException(string message, ApiErrorResponse<T> errorResponse, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            ErrorResponse = errorResponse;
            code = statusCode;
        }
}
public class JsonParserException : ApiBaseException<string>
{
    public JsonParserException(string message, ApiErrorResponse<String> errorResponse) : base(message, errorResponse, (HttpStatusCode)500){}
}
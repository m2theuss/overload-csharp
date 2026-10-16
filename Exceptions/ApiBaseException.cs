using System.Net;
using overload.Services.Shared;
namespace overload.Exceptions;

public abstract class ApiBaseException : Exception
{
    public HttpStatusCode code{get;}
    public ApiErrorResponse<String> apiErrorResponse{get;}
    protected ApiBaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        { 
            code = statusCode;
        }
}
public class JsonParserException : ApiBaseException
{
    public JsonParserException(string message) : base(message, (HttpStatusCode)500){}
}
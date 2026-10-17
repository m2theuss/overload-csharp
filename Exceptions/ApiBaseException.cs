using System.Net;
using overload.Services.Shared;
namespace overload.Exceptions;

public abstract class ApiBaseException<T> : Exception
{
    public HttpStatusCode code{get;}
    protected ApiBaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            code = statusCode;
        }
}
public class JsonParserException : ApiBaseException<string>
{
    public JsonParserException(string message) : base(message, (HttpStatusCode)500){}
}
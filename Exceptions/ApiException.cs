using System.Net;

namespace overload.Exceptions;

public abstract class ApiException : Exception{
    HttpStatusCode code{get;}
    protected ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        { 
            code = statusCode;
        }
}
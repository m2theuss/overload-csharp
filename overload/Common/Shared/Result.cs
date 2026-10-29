using System.Text.Json.Serialization;

namespace overload.Services.Shared;
public class Result<T>
{
    
    public bool IsOk{get; init;}
    public String Title{get; init;} = string.Empty;
    public T? Data{get; init;}
    public List<string> ErrorsList {get;init;} = new();
    public string SingleError{get; set;} = string.Empty;
    [JsonIgnore]
    public ErrorCode Code{get;init;}
    
    public static Result<T> OkWithData(T data) => 
        new Result<T>
        {
            IsOk = true,
            Data = data
        };
    public static Result<string> OkWithMessage(string singleError) =>
        new Result<string>
        {
            IsOk = true,
            SingleError = singleError
        };
    public static Result<T> Error(string title, string singleError, ErrorCode code) => 
        new Result<T>
        {
            IsOk = false, 
            Title = title, 
            SingleError = singleError,
            Code = code
    };
    public static Result<T> Error(string title, ICollection<string> errorsList, ErrorCode code) => 
        new Result<T>{
            IsOk = false,
            Title = title,
            ErrorsList = [.. errorsList],
            Code = code
        };
  
}
public enum ErrorCode
{
    UsernameAlreadyExists,
    EmailAlreadyExists,
    UsernameOrEmailAlreadyExist,
    InvalidData,
    BadCredentials,
    InternalError,
    DatabaseSaveFailure,
    NameConflict,
    Unauthorized,
    NotFound
}
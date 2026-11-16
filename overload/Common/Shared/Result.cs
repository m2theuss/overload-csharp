using System.Text.Json.Serialization;

namespace overload.Services.Shared;
public class Result<T>
{
    
    public bool IsOk{get; init;}
    public String Title{get; init;} = string.Empty;
    public T? Data{get; init;}
    public List<string> ErrorsList {get;init;} = new();
    public string Detail{get; set;} = string.Empty;
    [JsonIgnore]
    public ErrorCode Code{get;init;}
    
    public static Result<T> OkWithData(T data) => 
        new Result<T>
        {
            IsOk = true,
            Data = data
        };
    public static Result<string> OkWithMessage(string detail) =>
        new Result<string>
        {
            IsOk = true,
            Detail = detail
        };
    public static Result<T> OkEmpty() => 
        new Result<T>(){
            IsOk = true
        };
    public static Result<T> Error(string title, string detail, ErrorCode code) => 
        new Result<T>
        {
            IsOk = false, 
            Title = title, 
            Detail = detail,
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
    Ok = 0,
    UsernameAlreadyExists,
    EmailAlreadyExists,
    UsernameOrEmailAlreadyExist,
    InvalidData,
    BadCredentials,
    InternalError,
    DatabaseSaveFailure,
    NameConflict,
    Unauthorized,
    NotFound,
    DataConflict
}
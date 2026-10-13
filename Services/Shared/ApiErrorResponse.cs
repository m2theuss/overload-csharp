using System.Text.Json.Serialization;

namespace overload.Services.Shared;
public class ApiErrorResponse<T>
{
    public required String Title {get; set;} 
    public required int Status {get; set;}
    public List<T> Details { get; set; } = new List<T>();
    public DateTime Timestamp { get; init; } = DateTime.Now;

    [JsonIgnore]
    public bool IsOk {get; set;}
}
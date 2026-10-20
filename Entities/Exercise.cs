namespace overload.Entities;

using System.Text.Json.Serialization;
using overload.Entities.Enums;
public class Exercise
{
    public string Name{get; set;} = string.Empty;   
    public TargetMuscle Muscle{get; set;}
    public ExerciseType Type{get; set;}
    [JsonIgnore]
    public User Author{get; set;}
    public bool IsPublic{get; set;}

    [JsonIgnore]
    public DateTime CreatedAt { get; init;} = DateTime.Now.ToUniversalTime();
}
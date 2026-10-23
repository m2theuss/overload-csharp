namespace overload.Entities;

using System.Text.Json.Serialization;
using overload.Entities.Enums;
public class Exercise
{
    public int Id {get; set;}
    public string Name{get; set;} = string.Empty;   
    public TargetMuscle Muscle{get; set;}
    public ExerciseType Type{get; set;}
    public User? Author{get; set;}
    public bool IsPublic{get; set;}
    public DateTime CreatedAt { get; init;} = DateTime.Now.ToUniversalTime();
}
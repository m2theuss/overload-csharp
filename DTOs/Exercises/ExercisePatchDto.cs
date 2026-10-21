using System.Text.Json.Serialization;
using overload.Entities.Enums;

namespace overload.DTOs.Exercises;

public class ExercisePatchDto
{
    public string? Name{get; set;}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TargetMuscle? Muscle{get; set;}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExerciseType? Type{get; set;}
    public bool? IsPublic{get; set;}

}
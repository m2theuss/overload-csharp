namespace overload.DTOs;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using overload.Entities.Enums;
public class ExerciseDto
{
    [Required(ErrorMessage = "Provide a name to the exercise")]
    public string? Name{get; set;}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required(ErrorMessage = "Provide a target muscle (see the doc. to know all availale types)")]
    public TargetMuscle? Muscle{get; set;}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required(ErrorMessage = "Provide a exercise type (see the doc. to know all availale types)")]
    public ExerciseType? Type{get; set;}

    [Required(ErrorMessage = "Provide a visibility to true or false")]
    public bool? IsPublic{get; set;}
}
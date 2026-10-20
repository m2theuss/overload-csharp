namespace overload.DTOs;

using overload.Entities.Enums;
public class ExerciseDto
{
    public string Name{get; set;}
    public TargetMuscle Muscle{get; set;}
    public ExerciseType Type{get; set;}
    public bool IsPublic{get; set;}
}
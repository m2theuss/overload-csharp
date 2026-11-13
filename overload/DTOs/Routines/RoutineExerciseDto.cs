namespace overload.DTOs.Routines;

public class RoutineExerciseDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public int Order { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
}

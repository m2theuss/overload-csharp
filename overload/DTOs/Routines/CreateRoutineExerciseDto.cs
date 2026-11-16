using System.ComponentModel.DataAnnotations;

namespace overload.DTOs.Routines;

public class CreateRoutineExerciseDto
{    
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Exercise ID is required.")]
    public int ExerciseId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Order cannot be negative.")]
    public int Order { get; set; }

    [Range(1, 50, ErrorMessage = "Sets must be between 1 and 50.")]
    public int Sets { get; set; }

    [Range(1, 500, ErrorMessage = "Reps must be greater than zero.")]
    public int Reps { get; set; }
}

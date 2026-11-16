using System.ComponentModel.DataAnnotations;

namespace overload.DTOs.Routines;

public class RoutineDto
{
    [Required(ErrorMessage = "The routine name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 100 characters.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "The day of the week is required.")]
    [EnumDataType(typeof(DayOfWeek), ErrorMessage = "Invalid day of the week.")]
    public DayOfWeek Day { get; set; }

    public bool IsPublic { get; set; }
}

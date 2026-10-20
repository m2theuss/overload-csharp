using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExerciceController(ExerciseService _exerciseService) : ControllerBase
{
    [HttpPost]
    public IActionResult addExercise(ExerciseDto dto)
    {
        var UserId = User.FindFirst("id")?.Value;
        if(string.IsNullOrEmpty(UserId))
        {
            return Unauthorized("User not recognized");
        }
        _exerciseService.create(UserId,dto);
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController(IExerciseService _exerciseService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddExercise(ExerciseDto dto)
    {
        var UserId = User.FindFirst("id")?.Value;
        if(string.IsNullOrEmpty(UserId))
        {
            return Unauthorized("User not recognized");
        }
        Console.Write("passou do uderfinfirst");
        await _exerciseService.Create(UserId, dto);
        return Ok();
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.DTOs.Exercises;
using overload.Services.Shared;
using overload.Services;
[Route("api/exercises")]
[ApiController]
[Authorize]
public class ExerciseController(IExerciseService _exerciseService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddExercise(ExerciseDto dto)
    {
        string? UserId = User.FindFirst("id")?.Value;
        if(string.IsNullOrEmpty(UserId))
        {
            return Unauthorized("User not recognized");
        }
        Result<string> result = await _exerciseService.CreateAsync(int.Parse(UserId), dto);
        int CodeInt = result.Code switch{
            ErrorCode.BadCredentials => 401,
            ErrorCode.NameConflict => 400,
            _ => 500
        };
        if (!result.IsOk)
        {
            return Problem(
                title: result.Title,
                detail: result.SingleError,
                statusCode: CodeInt
            );
        }
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetAllExercises()
    {
        string? strId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(strId))
        {
            return Unauthorized("User not recognized");
        }
        return Ok(await _exerciseService.GetAllAsync(int.Parse(strId)));
    }
    [HttpPatch("{exerciseId}")]
    public async Task<IActionResult> UpdateExercise(int exerciseId, ExercisePatchDto patchDto)
    {
        string? authorId = User.FindFirst("id")?.Value;
        if(authorId == null)
        {
            return Unauthorized("User not recognized");
        }
        Result<string> result = await _exerciseService.Update(exerciseId, patchDto, int.Parse(authorId));
        var ParsedCodeError = result.Code switch
        {
            ErrorCode.NotFound => 404,
            _ => 500
        };
        if (!result.IsOk)
        {
            return Problem(
                title: result.Title,
                detail: result.SingleError,
                statusCode: ParsedCodeError
            );
        }
        return Ok();
    }

}
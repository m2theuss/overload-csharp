using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.DTOs.Exercises;
using overload.Services.Shared;
using overload.Services;
using overload.Extensions;
[Route("api/exercises")]
[ApiController]
[Authorize]
public class ExerciseController(IExerciseService exerciseService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAsync(ExerciseDto dto)
    {
        string? UserId = User.FindFirst("id")?.Value ?? throw new InvalidOperationException("The claim 'id' was not found in the token");

        Result<string> result = await exerciseService.CreateAsync(int.Parse(UserId), dto);
        int CodeInt = result.Code switch{
            ErrorCode.BadCredentials => 401,
            ErrorCode.NameConflict => 400,
            _ => 500
        };
        if (!result.IsOk)
        {
            return Problem(
                title: result.Title,
                detail: result.Detail,
                statusCode: CodeInt
            );
        }
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        string? strId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(strId))
        {
            return Unauthorized("User not recognized");
        }
        return Ok(await exerciseService.GetAllAsync(int.Parse(strId)));
    }
    [HttpPatch("{exerciseId}")]
    public async Task<IActionResult> UpdateByIdAsync(int exerciseId, ExercisePatchDto patchDto)
    {
        string? authorId = User.FindFirst("id")?.Value;
        if(authorId == null)
        {
            return Unauthorized("User not recognized");
        }
        Result<string> result = await exerciseService.UpdateAsync(exerciseId, patchDto, int.Parse(authorId));
        var ParsedCodeError = result.Code switch
        {
            ErrorCode.NotFound => 404,
            _ => 500
        };
        if (!result.IsOk)
        {
            return Problem(
                title: result.Title,
                detail: result.Detail,
                statusCode: ParsedCodeError
            );
        }
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsync(int id)
    {
        User.GetUserIdFromToken();
        var result = await exerciseService.DeleteByIdAsync(id,User.GetUserIdFromToken());
        return result.ToActionResult(HttpContext);
    }
}
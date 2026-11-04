using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.Services;
using overload.Services.Shared;

namespace overload.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutineController(IRoutineService _routineService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task Create(RoutineDto routineDto)
    {
        UserHasValidId(out int userId);
        await _routineService.CreateRoutine(userId, routineDto);
    }
    [HttpPost("addRoutine")]
    public async Task<Result<string>> AddRoutineExercise(int exerciseId, int routineId, int order) // add a exercise into a routine
    {
        UserHasValidId(out int authorId);
        var result = await _routineService.AddRoutineExercise(authorId, exerciseId, order, routineId);
        return result;

    }
    private bool UserHasValidId(out int userId)
    {
        string? stringUserId = User.FindFirst("id")?.Value ?? throw new InvalidOperationException("The claim 'id' was not found in the token");
        if(int.TryParse(stringUserId, out int value))
        {
            userId = value;
            return true;
        }
        else
        {
            userId = -1;
            return false;
        }

    }
}
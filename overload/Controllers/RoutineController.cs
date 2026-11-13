using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.DTOs.Routines;
using overload.Entities;
using overload.Services;
using overload.Services.Shared;

namespace overload.Controllers;

[Route("api/routines")]
[ApiController]
public class RoutineController(IRoutineService _routineService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task Create(RoutineDto routineDto)
    {
        UserHasValidId(out int userId);
        await _routineService.CreateRoutineAsync(userId, routineDto);
    }

    [Authorize]
    //add a exercise into a routine
    [HttpPost("{routineId}/exercises")]
    public async Task<ActionResult> AddRoutineExerciseAsync([FromBody] RoutineExerciseDto dto, int routineId) // add a exercise into a routine
    {
        UserHasValidId(out int authorId);
        var result = await _routineService.AddRoutineExerciseAsync(dto, routineId, authorId);
        return result.IsOk ? Ok(result.Message) : BadRequest(result.Message);

    }
    private bool UserHasValidId(out int userId) // method for internal validation
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

    [Authorize]
    [HttpGet()]
    public async Task<ActionResult<ICollection<Routine>>> GetAllRoutinesById()
    {
        UserHasValidId(out int userId);
        return Ok(await _routineService.GetAllByIdAsync(userId));
    } 

    [Authorize]
    [HttpGet("{routineId}/exercises")]
    public async Task<Result<ICollection<RoutineExerciseDto>>> GetExercisesByRoutineId(int routineId)
    {
        UserHasValidId(out int userId);
        return await _routineService.GetExerciseByRoutineIdAsync(userId, routineId); // keep on here, for some reason dont return anyvalue
    }
}
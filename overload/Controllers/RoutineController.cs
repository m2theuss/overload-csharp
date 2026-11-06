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
        await _routineService.CreateRoutine(userId, routineDto);
    }

    //add a exercise into a routine
    [HttpPost("{routineId}/exercises")]
    public async Task<ActionResult> AddRoutineExercise([FromBody] RoutineExerciseDto dto, int routineId) // add a exercise into a routine
    {
        UserHasValidId(out int authorId);
        var result = await _routineService.AddRoutineExercise(dto, routineId, authorId);
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
    [HttpGet("users/{anonId}/routines")]
    public ActionResult<ICollection<Routine>> GetAllRoutinesById(int anonId)
    {
        UserHasValidId(out int userId);
        if(userId != anonId)
        {
            return NotFound();
        }
        return Ok(_routineService.GetAllById(userId));
    } 
}
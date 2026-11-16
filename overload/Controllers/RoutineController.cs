using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.DTOs.Routines;
using overload.Entities;
using overload.Extensions;
using overload.Services;
using overload.Services.Shared;
namespace overload.Controllers;

[Route("api/routines")]
[ApiController]
public class RoutineController(IRoutineService _routineService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(RoutineDto routineDto)
    {
        var result = await _routineService.CreateRoutineAsync(User.GetUserIdFromToken(), routineDto);
        if (!result.IsOk)
        {
            int integerCode = result.Code switch{
                ErrorCode.DatabaseSaveFailure => 500,
                ErrorCode.NotFound => 404,
                ErrorCode.NameConflict => 409,
                _ => 500
            };
            return Problem(
                detail: result.Detail,
                title: result.Title,
                statusCode: integerCode
            );
        }
        return Ok("Routine has been created");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllRoutines()
    {
        var result = await _routineService.GetAllRoutinesByUserIdAsync(User.GetUserIdFromToken());
        return result.ToActionResult(HttpContext);
    } 

    [Authorize]
    [HttpPost("{routineId}/exercises")]
    public async Task<IActionResult> AddRoutineExerciseAsync(CreateRoutineExerciseDto dto, int routineId) // add a exercise into a routine
    {
        var result = await _routineService.AddRoutineExerciseAsync(dto, routineId, User.GetUserIdFromToken());
        return result.ToActionResult(HttpContext);
    }

    [Authorize]
    [HttpGet("{routineId}/exercises")]
    public async Task<IActionResult> GetExercisesByRoutineId(int routineId)
    {
        var result = await _routineService.GetAllExerciseByRoutineIdAsync(routineId: routineId, userId: User.GetUserIdFromToken());
        return result.ToActionResult(HttpContext);
    }
}
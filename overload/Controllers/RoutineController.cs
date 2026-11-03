using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.Services;

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
    public async Task AddRoutineExercise(int exerciseId, int routineId, int Order)
    {
        UserHasValidId(out int userId);
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
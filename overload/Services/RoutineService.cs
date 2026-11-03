using Microsoft.AspNetCore.Authorization;
using overload.Data;
using overload.DTOs;
using overload.Entities;
using overload.Services.Shared;
namespace overload.Services;
public interface IRoutineService
{
    public Task<Result<string>> CreateRoutine(int AuthorId, RoutineDto routineDto);
}
public class RoutineService(MyContext _myContext) : IRoutineService
{
    public async Task<Result<string>> CreateRoutine(int AuthorId, RoutineDto routineDto)
    {
        Routine routineExercise = new Routine
        {
            Name = routineDto.Name,
            Day = routineDto.Day,
            IsPublic = routineDto.IsPublic,
            AuthorId = AuthorId
        };
        _myContext.routines.Add(routineExercise);
        if(await _myContext.SaveChangesAsync() > 0)
        {
            return Result<string>.OkWithMessage("Routine has been created");
        }
        return Result<string>.Error("Database problem", "A error while saving the changes happen", ErrorCode.DatabaseSaveFailure);
    }
}
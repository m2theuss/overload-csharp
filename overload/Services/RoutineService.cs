using Microsoft.AspNetCore.Authorization;
using overload.Data;
using overload.DTOs;
using overload.Entities;
using overload.Services.Shared;
namespace overload.Services;
public interface IRoutineService
{
    public Task<Result<string>> CreateRoutine(int authorId, RoutineDto routineDto);
    public Task<Result<string>> AddRoutineExercise(int authorId, int exerciseId, int Order, int routineId);
}
public class RoutineService(MyContext _myContext, IExerciseService _exerciseService) : IRoutineService
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
        return Result<string>.Error("Database problem", "A error happen when try save the changes", ErrorCode.DatabaseSaveFailure);
    }
    public async Task<Result<string>> AddRoutineExercise(int authorId, int exerciseId, int order, int routineId)
    {
        Exercise? exercise = await _exerciseService.FindExerciseById(authorId);
        Routine? routine = await FindRoutineById(routineId);
        if(exercise == null || exercise.AuthorId != authorId || routine == null || routine.AuthorId == authorId)
        {
            return Result<string>.Error("Not found","The requested exercise could not be found.",ErrorCode.NotFound);
        }
        RoutineExercise routineExercise = new RoutineExercise
        {
            ExerciseId = exercise.AuthorId,
            Order = order,
            RoutineId = routine.AuthorId
        };
        _myContext.routine_exercises.Add(routineExercise);
        var result = await _myContext.SaveChangesAsync();
        return result > 0 ? Result<string>.OkWithMessage("Exercise has been added to the routine")
            : Result<string>.Error("Database problem", "A error happen when try save the changes", ErrorCode.DatabaseSaveFailure);
    }
    public async Task<Routine?> FindRoutineById(int routineId)
    {
        return _myContext.routines.FirstOrDefault(e => e.Id == routineId);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.DTOs;
using overload.DTOs.Routines;
using overload.Entities;
using overload.Services.Shared;
namespace overload.Services;

public interface IRoutineService
{
    public Task<Result<string>> CreateRoutineAsync(int authorId, RoutineDto routineDto);
    public Task<Result<string>> AddRoutineExercise(RoutineExerciseDto dto, int routineId, int userId);
    public ICollection<Routine> GetAllById(int userId);
}
public class RoutineService(MyContext _myContext, IExerciseService _exerciseService, IUserService userService) : IRoutineService
{
    public async Task<Result<string>> CreateRoutineAsync(int AuthorId, RoutineDto routineDto)
    {
        if(userService.FindUserById(AuthorId) == null)
        {
            return Result<string>.Error("User not found", "The send id of user was note found", ErrorCode.NotFound);
        }
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
    public async Task<Result<string>> AddRoutineExercise(RoutineExerciseDto dto, int routineId, int userId)
    {
        Exercise? exercise = await _exerciseService.FindExerciseById(dto.ExerciseId);
        Routine? routine = await FindRoutineById(routineId);

        if (routine == null || exercise == null || routine.AuthorId != userId || exercise.AuthorId != userId)
        {
            return Result<string>.Error("Not found", "The requested exercise could not be found.", ErrorCode.NotFound);
        }
        RoutineExercise routineExercise = new RoutineExercise
        {
            ExerciseId = dto.ExerciseId,
            Order = dto.Order,
            RoutineId = routineId
        };
        if (await IsAlreadyRegistred(routineExercise))
        {
            return Result<string>.Error("Data conflict", "Almost have a exercise with that id registred on this routine", ErrorCode.DataConflict);
        }
        await ResolveOrder(routineExercise);
        _myContext.routine_exercises.Add(routineExercise);
        var result = await _myContext.SaveChangesAsync();
        return result > 0 ? Result<string>.OkWithMessage("Exercise has been added to the routine")
            : Result<string>.Error("Database problem", "A error happen when try save the changes", ErrorCode.DatabaseSaveFailure);
    }
    private async Task<Routine?> FindRoutineById(int routineId)
    {
        return _myContext.routines.FirstOrDefault(e => e.Id == routineId);
    }
    private async Task ResolveOrder(RoutineExercise newRoutineExercise)
    {
        int amount = _myContext.routine_exercises.Where(e => e.RoutineId == newRoutineExercise.RoutineId).Count();
        if (amount == 0 || newRoutineExercise.Order > amount || newRoutineExercise.Order == 0)
        {
            newRoutineExercise.Order = ++amount;
            return;
        }
        if (amount == newRoutineExercise.Order)
        {
            RoutineExercise lastInOrder = await _myContext.routine_exercises.FirstOrDefaultAsync(e => e.Order == amount)
                ?? throw new KeyNotFoundException("A error happen when try to replace the last routine in order");
            newRoutineExercise.Order = lastInOrder.Order;
            lastInOrder.Order = ++amount;
            await _myContext.SaveChangesAsync();
            return;
        }
        RoutineExercise replacedOrderRoutine = await _myContext.routine_exercises.FirstOrDefaultAsync(e => e.Order == newRoutineExercise.Order)
            ?? throw new KeyNotFoundException("A error happen when try to get the exsercise to be replaced");
        newRoutineExercise.Order = replacedOrderRoutine.Order;
        replacedOrderRoutine.Order = ++amount;
        await _myContext.SaveChangesAsync();
    }
    private async Task<bool> IsAlreadyRegistred(RoutineExercise newRoutineExercise)
    {
        var result = await _myContext.routine_exercises.FirstOrDefaultAsync(e => e.RoutineId == newRoutineExercise.RoutineId
            && e.ExerciseId == newRoutineExercise.ExerciseId);
        return result != null ? true : false;
    }
    public ICollection<Routine> GetAllById(int userId)
    {
        return _myContext.routines.Where(e => e.AuthorId == userId).ToList();
    }
}
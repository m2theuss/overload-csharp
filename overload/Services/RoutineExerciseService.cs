using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.DTOs.Routines;
using overload.Entities;
using overload.Services.Shared;

namespace overload.Services;

public interface IRoutineExerciseService
{
    public Task<Result<ICollection<RoutineExerciseDtoResponse>>> FindAllExercisesFromRoutineAsync(int routineId, int userId, CancellationToken cancellationToken = default);
}
public class RoutineExerciseService(MyContext myContext) : IRoutineExerciseService
{
    public async Task<Result<ICollection<RoutineExerciseDtoResponse>>> FindAllExercisesFromRoutineAsync(int routineId, int userId, CancellationToken cancellationToken = default)
    {
        var result = await myContext.routine_exercises.AsNoTracking().
            Where(e => e.RoutineId == routineId && e.Routine!.AuthorId == userId).OrderBy(e => e.Order).
            Select(e => 
                new RoutineExerciseDtoResponse
                {
                    Id = e.Id,  
                    ExerciseId = e.ExerciseId, 
                    ExerciseName = e.ExerciseItem!.Name,
                    Order = e.Order,
                    Sets = e.Sets,
                    Reps = e.Reps
                }
        ).ToListAsync(cancellationToken);
        return result.Count <= 0 ? Result<ICollection<RoutineExerciseDtoResponse>>.Error("Not Found", "Routine id was not found", ErrorCode.NotFound)
            : Result<ICollection<RoutineExerciseDtoResponse>>.OkWithData(result);

    }
}
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.DTOs.Routines;
using overload.Entities;
using overload.Services.Shared;

namespace overload.Services;

public interface IRoutineExerciseService
{
    public Task<Result<ICollection<RoutineExerciseDto>>> FindAllExercisesFromRoutineAsync(int routineId, int userId, CancellationToken cancellationToken = default);
}
public class RoutineExerciseService(MyContext myContext) : IRoutineExerciseService
{
    public async Task<Result<ICollection<RoutineExerciseDto>>> FindAllExercisesFromRoutineAsync(int routineId, int userId, CancellationToken cancellationToken = default)
    {
        var result = await myContext.routine_exercises.AsNoTracking().
            Where(e => e.RoutineId == routineId || e.ExerciseItem!.AuthorId == userId).
            Select(e => 
                new RoutineExerciseDto
                {
                    Id = e.Id, 
                    ExerciseId = e.ExerciseId, 
                    ExerciseName = e.ExerciseItem!.Name,
                    Order = e.Order,
                    Sets = e.Sets,
                    Reps = e.Reps
                }
        ).ToListAsync(cancellationToken);
        return result.Count == 0 ? Result<ICollection<RoutineExerciseDto>>.Error("Not Found", "Any routines exercise was find",ErrorCode.NotFound)
            : Result<ICollection<RoutineExerciseDto>>.OkWithData(result);

    }
}
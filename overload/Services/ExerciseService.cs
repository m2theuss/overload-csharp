namespace overload.Services;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.DTOs;
using overload.DTOs.Exercises;
using overload.Entities;
using overload.Services;
using overload.Services.Shared;
using overload.Entities.Enums;
public interface IExerciseService
{
    public Task<Result<string>> CreateAsync(int userId, ExerciseDto dto);
    public Task<List<Exercise>> GetAllAsync(int UserId);
    public Task<Result<string>> UpdateAsync(int exerciseId, ExercisePatchDto patchDto, int authorId);
    public Task<Exercise?> FindExerciseById(int exerciseId);
    public Task<Result<string>> DeleteByIdAsync(int id, int userId);

}
public class ExerciseService(MyContext myContext, IUserService userService) : IExerciseService
{
    public async Task<Result<string>> CreateAsync(int UserId, ExerciseDto dto)
    {
        User user = userService.FindUserById(UserId)!;
        if(user == null)
        {
            return Result<string>.Error("User dont exist", "Cannot locate the user with that specific id provided by token", ErrorCode.BadCredentials);
        }
        Exercise newExerc = new Exercise{
            Name = dto.Name!,
            Author = user,
            Type = dto.Type!.Value,
            Muscle = dto.Muscle!.Value,
            IsPublic = dto.IsPublic!.Value
        };
        if (!await ExerciseNameIsAvailable(newExerc.Name, newExerc.Author.Id))
        {
          return Result<string>.Error("Exercise name conflict", "The user almost have a excercise with that name", ErrorCode.NameConflict);
        }
        myContext.Add<Exercise>(newExerc);
        await myContext.SaveChangesAsync();
        return Result<string>.OkWithMessage("Exercise created");
    }
    private async Task<bool> ExerciseNameIsAvailable(string name, int authorId)
    {
        return !await myContext.exercises.AnyAsync(e => e.Name == name && e.Author!.Id == authorId);
    }
    public async Task<List<Exercise>> GetAllAsync(int UserId)
    {
        return await myContext.exercises.Where(e => e.Author!.Id == UserId).ToListAsync();
    }
    public async Task<Result<string>> UpdateAsync(int ExerciseId, ExercisePatchDto patchDto, int authorId)
    {
        Exercise? tmpEx = myContext.exercises.FirstOrDefault(e => e.Id == ExerciseId);
        if(tmpEx == null || tmpEx.AuthorId != authorId)
        {
            return Result<string>.Error("Data not found", "The exercise provide was not found", ErrorCode.NotFound);
        }
        if(patchDto.IsPublic is bool IsPublic) tmpEx.IsPublic = IsPublic;
        if(patchDto.Muscle is TargetMuscle muscle) tmpEx.Muscle = muscle;
        if(patchDto.Type is ExerciseType type) tmpEx.Type = type;
        if(patchDto.Name is string name) tmpEx.Name = name;
        await myContext.SaveChangesAsync();
        return Result<string>.OkWithMessage("Exercise updated");
    }
    public async Task<Exercise?> FindExerciseById(int exerciseId)
    {
        return await myContext.exercises.FirstOrDefaultAsync(e => e.Id == exerciseId);
    }
    public async Task<Result<string>> DeleteByIdAsync(int id, int userId)
    {
        Exercise? exercise = await myContext.exercises.FirstOrDefaultAsync(e => e.Id == id && e.AuthorId == userId);
        if(exercise is null)
        {
            return Result<string>.Error("Not found", "Was not posible find that exercise id", ErrorCode.NotFound);
        }
        myContext.Remove(exercise!);
        await myContext.SaveChangesAsync();
        return Result<string>.OkEmpty();
    }
    

}
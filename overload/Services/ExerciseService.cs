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
    public Task<Result<string>> CreateAsync(int UserId, ExerciseDto dto);
    public Task<List<Exercise>> GetAllAsync(int UserId);
    public Task<Result<string>> Update(int ExerciseId, ExercisePatchDto patchDto, int authorId);

}
public class ExerciseService(MyContext _myContext, IUserService _userService) : IExerciseService
{
    public async Task<Result<string>> CreateAsync(int UserId, ExerciseDto dto)
    {
        User user = _userService.FindUserById(UserId)!;
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
        _myContext.Add<Exercise>(newExerc);
        await _myContext.SaveChangesAsync();
        return Result<string>.OkWithMessage("Exercise created");
    }
    private async Task<bool> ExerciseNameIsAvailable(string name, int authorId)
    {
        return !await _myContext.exercises.AnyAsync(e => e.Name == name && e.Author!.Id == authorId);
    }
    public async Task<List<Exercise>> GetAllAsync(int UserId)
    {
        return await _myContext.exercises.Where(e => e.Author!.Id == UserId).ToListAsync();
    }
    public async Task<Result<string>> Update(int ExerciseId, ExercisePatchDto patchDto, int authorId)
    {
        Exercise? tmpEx = _myContext.exercises.FirstOrDefault(e => e.Id == ExerciseId);
        if(tmpEx == null || tmpEx.AuthorId != authorId)
        {
            return Result<string>.Error("Data not found", "The exercise provide was not found", ErrorCode.NotFound);
        }
        if(patchDto.IsPublic is bool IsPublic) tmpEx.IsPublic = IsPublic;
        if(patchDto.Muscle is TargetMuscle muscle) tmpEx.Muscle = muscle;
        if(patchDto.Type is ExerciseType type) tmpEx.Type = type;
        if(patchDto.Name is string name) tmpEx.Name = name;
        await _myContext.SaveChangesAsync();
        return Result<string>.OkWithMessage("Exercise update");
    }
}
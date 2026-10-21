using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.DTOs;
using overload.DTOs.Exercises;
using overload.Entities;
using overload.Services;
using overload.Services.Shared;
public interface IExerciseService
{
    public Task<Result<string>> CreateAsync(int UserId, ExerciseDto dto);
    public Task<List<Exercise>> GetAllAsync(int UserId);
    public void Update(int ExerciseId, ExercisePatchDto patchDto);

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
        return Result<string>.Ok("Exercise created");
    }
    private async Task<bool> ExerciseNameIsAvailable(string name, int authorId)
    {
        return !await _myContext.exercises.AnyAsync(e => e.Name == name && e.Author.Id == authorId);
    }
    public async Task<List<Exercise>> GetAllAsync(int UserId)
    {
        return await _myContext.exercises.Where(e => e.Author.Id == UserId).ToListAsync();
    }
    public void Update(int ExerciseId, ExercisePatchDto patchDto)
    {
        
    }
}
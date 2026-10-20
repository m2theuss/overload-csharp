using overload.Data;
using overload.DTOs;
using overload.Entities;
using overload.Services;
public interface IExerciseService
{
    public Task Create(string id, ExerciseDto dto);
}
public class ExerciseService(MyContext _myContext, IUserService _userService) : IExerciseService
{
    public async Task Create(string id, ExerciseDto dto)
    {
        User user = _userService.FindUserById(int.Parse(id));
        if(user == null)
        {
            return;
        }
        Exercise newExerc = new Exercise{
            Name = dto.Name,
            Author = user,
            Type = dto.Type,
            Muscle = dto.Muscle,
            IsPublic = dto.IsPublic
        };
        _myContext.Add<Exercise>(newExerc);
        await _myContext.SaveChangesAsync();
    }
}
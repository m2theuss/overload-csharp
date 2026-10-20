using System;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
using overload.Services.Shared;
using BC = BCrypt.Net.BCrypt;
namespace overload.Services;

public interface IUserService
{
    public Task<Result<string>> createAsync(User user);
    public User? FindUserById(int id);
}
public class UserService(MyContext _myContext) : IUserService
{  
    public async Task<Result<string>> createAsync(User user)
    {   
        Result<string> result = CheckForRegistrationConflicts(user.Username, user.Email);
        if(!result.IsOk){ // The method check...() just register all the enough information about whats happen before sent it
            return result;
        }
        user.Password = BC.HashPassword(user.Password);
        _myContext.Add<User>(user);
        if(await _myContext.SaveChangesAsync() < 0){ // less than 0 means that the update returns 0 affected rows...
            return Result<string>.Error(
                "Internal error",
                "A error happen when try save the changes, try again or later",
                ErrorCode.DatabaseSaveFailure
            );
        }
        return result;
    }
    private Result<string> CheckForRegistrationConflicts(string username, string email){
        var conflict = _myContext.Users.Where((u => u.Username == username || u.Email == email))
                        .Select(u => new {u.Email, u.Username}).ToList();

        bool usernameExist = conflict.Any(u => u.Username == username);
        bool emailExist = conflict.Any(u => u.Email == email);

        var details = new List<string>();
        if (usernameExist)
        {
            details.Add("This username isn't available");
        }
        if (emailExist)
        {
            details.Add("This email isn't available");
        }
        return details.Count == 0 ? 
            Result<string>.Ok("Valid") : 
            Result<string>.Error(
                "Conflict",
                details,
                ErrorCode.UsernameOrEmailAlreadyExist
            );
    }
    public User? FindUserById(int id)
    {
        return _myContext.Users.FirstOrDefault(u => u.Id == id);
    }
}
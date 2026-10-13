using System;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
using overload.Services.Shared;
using BC = BCrypt.Net.BCrypt;
namespace overload.Services;

public interface IUserService
{
    public Task<ApiErrorResponse<String>> create(User user);
}
public class UserService(MyContext _myContext) : IUserService
{  
    public async Task<ApiErrorResponse<String>> create(User user)
    {   
        ApiErrorResponse<String> check = CheckForRegistrationConflicts(user.Username, user.Email);
        if(!check.IsOk){ // The method checkfor... automatic register the enough information about whats happen before sent it
            return check;
        }
        user.Password = BC.HashPassword(user.Password);
        _myContext.Add<User>(user);
        if(await _myContext.SaveChangesAsync() > 0){
            check.IsOk = true;
            return check;
        }
        check.IsOk = false;
        return check;
    }
    public ApiErrorResponse<String> CheckForRegistrationConflicts(string username, string email){
        var conflict = _myContext.users.Where((u => u.Username == username || u.Email == email))
                        .Select(u => new {u.Email, u.Username}).ToList();

        bool usernameExist = conflict.Any(u => u.Username == username);
        bool emailExist = conflict.Any(u => u.Email == email);
        ApiErrorResponse<String> error = new ApiErrorResponse<string>{
            Title = "One or more fields is unvailable",
            Status = 400
        };
        if(usernameExist){
            error.Details.Add("This username isn't available");
        }
        if(emailExist){
            error.Details.Add("This email isn't available");
        }
        if(error.Details.Count > 0){ //biggest than zero means that some of the messages above was filled in ErrorResponse class
            error.IsOk = false;
            return error;
        }
        error.IsOk = true;
        return error;
    }
}
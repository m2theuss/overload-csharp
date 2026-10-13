using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.DTOs;
using overload.Entities;
using overload.Services.Shared;
using BC = BCrypt.Net.BCrypt;

namespace overload.Services;

public interface IAuthService{
    public Task<ApiErrorResponse<string>> Login(UserDto userDto);
}
public class AuthService(MyContext _myContext) : IAuthService{
    public async Task<ApiErrorResponse<string>> Login(UserDto userDto){
        bool emailHasContent = !string.IsNullOrEmpty(userDto.Email);
        bool usernameHasContent = !string.IsNullOrEmpty(userDto.Username);
        if((emailHasContent && usernameHasContent) || (!emailHasContent && !usernameHasContent)){
            return new ApiErrorResponse<string>{
                Title = "Parameter conflicts",
                Status = 400,
                Details = new List<String> {"Please provinder either email or username"},
                IsOk = false
            };
        }
        User? result;
        if(emailHasContent){
            result = await _myContext.users.FirstOrDefaultAsync(e => e.Email == userDto.Email);
        }else{
            result = await _myContext.users.FirstOrDefaultAsync(e => e.Username == userDto.Username);
        }

        if(result != null){
            if(BC.Verify(userDto.Password, result.Password)){
                return new ApiErrorResponse<string>{
                    Title = "Credentials are right",
                    Status = 200,
                    Details = new List<String> {"Your token..."},
                    IsOk = true
                };
            }
        }
        return new ApiErrorResponse<string>{
            Title = "Bad Credentials",
            Status = 401,
            Details = new List<String> {"Some credentials are incorrect"},
            IsOk = false
        };
    }
}
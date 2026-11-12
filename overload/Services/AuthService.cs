namespace overload.Services;

using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.DTOs;
using overload.Entities;
using overload.Services.Shared;
using overload.Utils;
using BC = BCrypt.Net.BCrypt;


public interface IAuthService{
    public Task<Result<string>> LoginAsync(UserDto userDto);
    public bool UserInfo();
}
public class AuthService(MyContext _myContext, IJwtUtils _jwtUtils) : IAuthService{

    public async Task<Result<string>> LoginAsync(UserDto userDto){
        bool emailHasContent = !string.IsNullOrEmpty(userDto.Email);
        bool usernameHasContent = !string.IsNullOrEmpty(userDto.Username);
        if((emailHasContent && usernameHasContent) || (!emailHasContent && !usernameHasContent)){
            return Result<string>.Error("Conflict","Please provinder either a e-mail or username",ErrorCode.InvalidData);
        }
        User? result;
        if(emailHasContent){
            result = await _myContext.users.FirstOrDefaultAsync(e => e.Email == userDto.Email || e.Username == userDto.Username);
        }else{
            result = await _myContext.users.FirstOrDefaultAsync(e => e.Username == userDto.Username);
        }

        if(result != null){
            if(BC.Verify(userDto.Password, result.Password)){
                var generatedToken = _jwtUtils.generateJwtToken(result);
                if(generatedToken != null)
                {
                    return Result<string>.OkWithData(generatedToken);
                }
                return Result<string>.Error("Internal error", "Some problem happen when try to generete the token",ErrorCode.InternalError);
            }
        }
        return Result<string>.Error("Bad Credentials", "Some credentials are incorrect", ErrorCode.BadCredentials);
    }
    public bool UserInfo()
    {
        return true;
    }
}
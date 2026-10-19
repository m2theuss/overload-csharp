namespace overload.Controllers;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.Services;
using overload.Services.Shared;
using overload.Utils;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase{

    [HttpPost("login")]
    public async Task<IActionResult> UserLogin(UserDto userDto){
        Result<string> result = await _authService.Login(userDto);
        if (!result.IsOk)
        {
            var errorCode = result.Code switch
            {
                ErrorCode.InvalidData => 409,
                ErrorCode.BadCredentials => 401,
                _ => 500
            };
            return Problem(
                title: result.Title,
                detail: result.SingleError,
                statusCode: errorCode
            );
        }
        var structResult = new TokenResponse(result.Data!, "Bearer", 3000);
        return Ok(structResult);
    }
}
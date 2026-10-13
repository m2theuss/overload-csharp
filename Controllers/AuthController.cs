namespace overload.Controllers;
using Microsoft.AspNetCore.Mvc;
using overload.DTOs;
using overload.Services;
using overload.Services.Shared;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase{

    [HttpPost("login")]
    public async Task<IActionResult> userLogin(UserDto userDto){
        ApiErrorResponse<string> result = await _authService.Login(userDto);
        return StatusCode(result.Status, result);
    }
}
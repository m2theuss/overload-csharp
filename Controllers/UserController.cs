namespace overload.Controllers;
using Microsoft.AspNetCore.Mvc;
using overload.Entities;
using overload.Exceptions;
using overload.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpGet]
    public String Hello()
    {
        return "Overload - Workout Tracker v1.0";
    }
    [HttpPost]
    public async Task<IActionResult> AddUser(User user)
    {
        var result = await _userService.create(user);
        if(result.IsOk){
            return Ok(new {Message = "User created successfully"});  
        }
        return StatusCode(result.Status, result);
    }
    
}
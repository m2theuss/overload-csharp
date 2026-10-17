namespace overload.Controllers;

using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using overload.Entities;
using overload.Exceptions;
using overload.Services;
using overload.Services.Shared;

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
    public async Task<ActionResult<string>> AddUser(User user)
    {
        Result<string> result = await _userService.createAsync(user);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if(result.IsOk){
            return Ok(new {Message = "User created successfully"});  
        }
        int status = result.Code switch
        {
            ErrorCode.UsernameOrEmailAlreadyExist => 400,
            _ => 500
        };
        string errorDetails = string.Join(", ", result.ErrorsList);
        return Problem(
            title: result.Title,
            statusCode: status,
            detail: errorDetails
        );
    }
    
}
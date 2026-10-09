namespace overload.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
using overload.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private IUserService _userService;
    public UserController(IUserService userService)
    {
        this._userService = userService; 
    }
    [HttpGet]
    public String heelo()
    {
        return "Overload - workout tracker v1.0";
    }
    [HttpPost]
    public IActionResult addUser(User user)
    {
        _userService.create(user);
        return Ok();  
    }
}
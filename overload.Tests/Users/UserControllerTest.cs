using Microsoft.AspNetCore.Mvc;
using Moq;
using overload.Controllers;
using overload.DTOs;
using overload.Entities;
using overload.Services;
using overload.Services.Shared;
using Xunit;
public class UserControllerTest
{
    [Fact]
    public async Task MustCreateUserWhenDataIsValid()
    {
        var mock = new Mock<IUserService>();
        mock.Setup(s => s.createAsync(
            It.IsAny<User>()))
            .ReturnsAsync(Result<string>.Ok("User created"));
        var controller = new UserController(mock.Object);
        User userDto = new()
        {
            Id = 1, 
            FirstName = "João", 
            LastName = "Silva", 
            Username = "joaosilva", 
            Password = "SenhaSegura123", 
            BirthDate = new DateOnly(1995, 5, 15)
        };
        var result = await controller.AddUser(userDto);
        Assert.IsType<OkObjectResult>(result.Result);
    }
}
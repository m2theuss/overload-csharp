using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using overload.DTOs;
using overload.Entities;
using overload.Fixtures;
using overload.Services;
using overload.Services.Shared;
using overload.Tests.Common;
using overload.Tests.Data;
using overload.Utils;
using BC = BCrypt.Net.BCrypt;

namespace overload.Tests.Services;

[Collection("Integration Test")]
public class AuthServiceTest : IntegrationTestBase
{
    public AuthServiceTest(TestFixture fixture) : base(fixture){}
    private const string validEmail = "email@test.com";
    private const string validUsername = "username";
    private const string emptyValue = "";
    
    [Fact]
    public async Task Should_FailLoginUser_When_BothUsernameAndEmailAreSubmited()
    {
        var jwtMock = new Mock<JwtUtils>();
        AuthService service = new AuthService(Handler!.Context!, jwtMock.Object);

        var result = await service.LoginAsync(GenerateUserDto("name","email","password"));

        Assert.False(result.IsOk);
        Assert.Equal(ErrorCode.InvalidData,result.Code);
    }

    [Theory]
    [InlineData(validEmail,emptyValue)]
    [InlineData(emptyValue, validUsername)]
    public async Task Should_LoginUsingEmailOrUsername_When_CredentialsAreRight(string emailOrEmpty, string usernameOrEmpty)
    {
        var jwtMock = new Mock<IJwtUtils>();
        jwtMock.Setup(e => e.generateJwtToken(It.IsAny<User>())).Returns("jwtRepresentation");
        AuthService service = new AuthService(Handler!.Context!, jwtMock.Object);

        UserDto dto = GenerateUserDto(usernameOrEmpty, emailOrEmpty, "password");
        User user = GenerateUser(validUsername, validEmail, dto.Password); //inside this method the password is encoded using bcript;

        Handler!.Context!.users.Add(user);
        await Handler.Context.SaveChangesAsync();
        
        var result = await service.LoginAsync(dto);
        //Assert.True(result.IsOk);
        Assert.Equal(ErrorCode.Ok,result.Code);
        Assert.Equal("jwtRepresentation",result.Data);
    }
    private static UserDto GenerateUserDto(string username, string email, string password)
    {
        return new UserDto{
            Username = username ?? default!,
            Email = email ?? default!,
            Password = password ?? default!
        };
    }
    private static User GenerateUser(string username, string email, string password){
        return new User
        {
            FirstName = "Nome",
            LastName = "Sobrenome",
            Username = username,
            Email = email,
            Password = BC.HashPassword(password),
            BirthDate = new DateOnly(2000, 1, 1)
        };
    }
}
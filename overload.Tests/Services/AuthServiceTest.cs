using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using overload.DTOs;
using overload.Entities;
using overload.Fixtures;
using overload.Services;
using overload.Services.Shared;
using overload.Tests.Data;
using overload.Utils;
using BC = BCrypt.Net.BCrypt;

namespace overload.Tests.Services;

[Collection("Integration Test")]
public class AuthServiceTest : IAsyncLifetime
{
    private AsyncServiceScope _scope; //this allow that each test have a clean context handler and be able to use dispose on that
    private IContextHandler? Handler{get;set;}
    public AuthServiceTest(TestFixture fixture)
    {
        _scope = fixture.Provider.CreateAsyncScope();
        Handler = _scope.ServiceProvider.GetRequiredService<IContextHandler>(); //when i call Icontext... inside the fixture this give me a non interface Context class
    }
    public async Task InitializeAsync()
    {
        await _scope.DisposeAsync();
        await Handler!.InitializeContextAsync();
    }
    public async Task DisposeAsync()
    {
        await Handler!.DisposeAsync();
    }
    
    [Fact]
    public async Task Should_FailLoginUser_When_BothUsernameAndEmailAreSubmited()
    {
        var jwtMock = new Mock<JwtUtils>();
        AuthService service = new AuthService(Handler!.Context!, jwtMock.Object);

        var result = await service.LoginAsync(GenerateUserDto("name","email","password"));

        Assert.False(result.IsOk);
        Assert.Equal(ErrorCode.InvalidData,result.Code);
    }

    [Fact]
    public async Task Should_Login_When_UserCredentialsAreRight()
    {
        var jwtMock = new Mock<JwtUtils>();
        jwtMock.Setup(e => e.generateJwtToken(It.IsAny<User>())).Returns("jwtRepresentation");
        AuthService service = new AuthService(Handler!.Context!, jwtMock.Object);

        UserDto dto = GenerateUserDto("username", string.Empty /*here must be empty*/, "password");
        User user = GenerateUser(dto.Username, "myemail@com.br", dto.Password); //inside this method the password is encoded using bcript;

        Handler!.Context!.users.Add(user);
        await Handler.Context.SaveChangesAsync();
        
        var result = await service.LoginAsync(dto);
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
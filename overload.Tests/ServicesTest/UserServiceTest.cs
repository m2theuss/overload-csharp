using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
using overload.Fixtures;
using overload.Services;
using overload.Tests.Common;
namespace overload.Tests.ServicesTest;

[Collection("Integration Test")]
public class UserServiceTest : IntegrationTestBase
{
    public UserServiceTest(TestFixture fixture) : base(fixture){}
    [Fact]
    public async Task Should_CreateUser_When_DataValid()
    {
        UserService service = new UserService(Handler.Context!);
        User user = BuildUser(10);

        var result = await service.createAsync(user);

        Assert.True(result.IsOk);
    }

    [Fact]
    public async Task Should_FailCreateUser_When_EmailOrUsernameAlreadyExists()
    {
        UserService service = new UserService(Handler!.Context!);
        User seed = await SeedUserAsync(10); 

        User user = new()
        {
            FirstName = seed.FirstName,
            LastName = seed.LastName,
            Username = seed.Username,
            Email = seed.Email,
            Password = seed.Password,
            BirthDate = seed.BirthDate
        };
        var result = await service.createAsync(user);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void Should_ReturnInvalidDataError_When_SomeUserFieldWasInvalid()
    {
        
    }
}
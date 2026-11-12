using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
using overload.Fixtures;
using overload.Services;
using overload.Services.Shared;
using overload.Tests.Common;
using overload.Tests.Data;
namespace overload.Tests.Services;

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
        Assert.Equal("Valid",result.Message);
    }

    [Fact]
    public async Task Should_FailCreateUser_When_EmailOrUsernameAlreadyExists()
    {
        UserService service = new UserService(Handler!.Context!);
        User seed = await SeedUserAsync(10); // here create and SAVE IN DB the "original" one user

        User user = new()
        {
            FirstName = seed.FirstName,
            LastName = seed.LastName,
            Username = seed.Username,
            Email = seed.Email,
            Password = seed.Password,
            BirthDate = seed.BirthDate
        };
        var result = await service.createAsync(user); // here i will try to save the same data again
        Assert.False(result.IsOk);
        Assert.Equal(ErrorCode.UsernameOrEmailAlreadyExist, result.Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidDataError_When_SomeUserFieldWasInvalid()
    {
        UserService userService = new UserService(Handler.Context!);
        var result = await userService.createAsync(new User{});
        Assert.Equal(ErrorCode.InvalidData, result.Code);
    }

    [Fact]
    public async Task Should_ReturnOk_When_UserSendDataWasRight()
    {
        UserService userService = new UserService(Handler.Context!);
        var result = await userService.createAsync(BuildUser(10));
        Assert.True(result.IsOk);
        Assert.Equal("Valid",result.Message);
    }

    [Fact]
    public async Task Should_FailCreateUser_When_NoDatabaseChangesWereMade()
    {
        var opt = Handler.Options;
        UserService userService = new UserService(new FailMyContext(opt!));
        var result = await userService.createAsync(BuildUser(10));
        Assert.Equal(ErrorCode.DatabaseSaveFailure, result.Code);
    }
}
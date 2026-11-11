using System.Net.NetworkInformation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using overload.DTOs.Routines;
using overload.Entities;
using overload.Fixtures;
using overload.Services;
using overload.Services.Shared;
using overload.Tests.TestUtils;

namespace overload.Tests.Routines;

[Collection("Integration Test")]
public class RoutineServiceTest : IAsyncLifetime
{
    public IContextHandler Handler{get;set;}
    public AsyncServiceScope _scope;
    
    public RoutineServiceTest(TestFixture fixture)
    {
        _scope = fixture.Provider.CreateAsyncScope();
        Handler = _scope.ServiceProvider.GetRequiredService<IContextHandler>();
    }

    public async Task InitializeAsync()
    {
        await Handler.InitializeContextAsync();
    }

    public async Task DisposeAsync()
    {
        await Handler.DisposeAsync();
        await _scope.DisposeAsync();
    }
    [Fact]
    public async Task Should_FailCreateRoutine_When_UserIdWasNotFound()
    {
        var exerciseService = new Mock<IExerciseService>();
        var userService = new Mock<IUserService>();

        userService.Setup(u => u.FindUserById(It.IsAny<int>())).Returns(new User{});

        RoutineService routineService = new RoutineService(Handler.Context!, exerciseService.Object, userService.Object);
        var result = await routineService.CreateRoutineAsync(1, new RoutineDto{});

        Assert.False(result.IsOk);
        Assert.Equal(ErrorCode.NotFound, result.Code);
    }
}
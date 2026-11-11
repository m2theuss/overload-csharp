using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using overload.Data;
using overload.DTOs.Routines;
using overload.Entities;
using overload.Fixtures;
using overload.Services;
using overload.Services.Shared;
using overload.Tests.Common;
using overload.Tests.Data;
namespace overload.Tests.Routines;

[Collection("Integration Test")]
public class RoutineServiceTest : IntegrationTestBase
{
    public RoutineServiceTest(TestFixture fixture) : base(fixture){}
    [Fact]
    public async Task Should_FailCreateRoutine_When_UserIdWasNotFound()
    {
        var exerciseService = new Mock<IExerciseService>();
        var userService = new Mock<IUserService>();
        userService.Setup(u => u.FindUserById(It.IsAny<int>())).Returns((User?)null);

        RoutineService routineService = new RoutineService(Handler.Context!, exerciseService.Object, userService.Object);
        var result = await routineService.CreateRoutineAsync(100, new RoutineDto{});

        Assert.False(result.IsOk);
        Assert.Equal(ErrorCode.NotFound, result.Code);
    }

    [Fact]
    public async Task Should_FailCreateRoutine_When_NoDatabaseChangesWereMade()
    {
        var exerciseServiceMock = new Mock<IExerciseService>();
        var userServiceMock = new Mock<IUserService>();
        var fakeContext = new FailMyContext(Handler!.Options!);

        userServiceMock.Setup(u => u.FindUserById(It.IsAny<int>())).Returns(new User{Id = 1});

        RoutineService routineService = new RoutineService(fakeContext, exerciseServiceMock.Object, userServiceMock.Object);
        var result = await routineService.CreateRoutineAsync(100, new RoutineDto{});

        Assert.False(result.IsOk);
    }
}
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
namespace overload.Tests.Services;

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

    [Theory]
    [InlineData(true,false,false,false)]
    public void Should_FailToAddRoutineExercise_When_RoutineOrExerciseDoesNotExistOrHasAnotherOwner(
        bool routineIsNull,
        bool exerciseIsNull,
        bool isRoutineAuthor,
        bool isExerciseAuthor)
    {
        var userId = 10;
        var otherUserId = 12;
        var routineId = 13;
        var dto = new RoutineExerciseDto{ExerciseId = 17};

        Routine routine = null;
        if(!routineIsNull) new Routine{Id = routineId, AuthorId = isRoutineAuthor ? userId : otherUserId};

        Exercise exercise = null;
        if(!exerciseIsNull) new Exercise{Id = dto.ExerciseId, AuthorId = isExerciseAuthor ? userId : otherUserId};

        var exerciseServiceMock = new Mock<IExerciseService>().Setup(e => e.FindExerciseById(It.IsAny<int>())).Returns(Task.FromResult(exercise));
        var routineServiceMock = new Mock<IRoutineService>().Setup(e => e.)
    }
}
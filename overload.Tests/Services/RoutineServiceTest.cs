using System.Net.NetworkInformation;
using System.Threading.Tasks;
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

        var routineExerciseService = new Mock<IRoutineExerciseService>();
        RoutineService routineService = new RoutineService(Handler.Context!, exerciseService.Object, userService.Object, routineExerciseService.Object);
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

        var routineExerciseService = new Mock<IRoutineExerciseService>();
        RoutineService routineService = new RoutineService(fakeContext, exerciseServiceMock.Object, userServiceMock.Object, routineExerciseService.Object);
        var result = await routineService.CreateRoutineAsync(100, new RoutineDto{});

        Assert.False(result.IsOk);
    }

    [Theory]
    [InlineData(true,false,false,false)]
    [InlineData(false,true,false,false)]
    [InlineData(false,false,true,false)]
    [InlineData(false,false,false,true)]
    public async Task Should_FailToAddRoutineExerciseAsync_When_RoutineOrExerciseDoesNotExistOrHasAnotherOwner(
        bool routineIsNull,
        bool exerciseIsNull,
        bool isRoutineFromAnotherUser,
        bool isExerciseFromAnotherUser
    )
    {
        //numbersId are random
        int routineId = 10;
        int authorId = 11;
        int otherUserId = 20;
        int exerciseId = 15;
        int userId = 20;

        Routine? routine = null;
        if(!routineIsNull) routine = new Routine{Id = routineId, AuthorId = isRoutineFromAnotherUser ? authorId : otherUserId}; 
        Exercise? exercise = null;
        if(!exerciseIsNull) exercise = new Exercise{Id = exerciseId, AuthorId = isExerciseFromAnotherUser ? authorId : otherUserId}; 

        var exerciseServiceMock = new Mock<IExerciseService>();
        exerciseServiceMock.Setup(e => e.FindExerciseById(It.IsAny<int>())).Returns(Task.FromResult(exercise));
        
        var userServiceMock = new Mock<IUserService>();
        
        if(!(routineIsNull && exerciseIsNull)) // I could used just the routineIsNull check or exerciseIsNull check, but, to be more readble i prefer use both
        {
            await SeedUserAsync(authorId);
            await SeedRoutineAsync(routineId, authorId);
        }
        
        var routineExerciseService = new Mock<IRoutineExerciseService>();
        var routineService = new RoutineService(Handler.Context!, exerciseServiceMock.Object, userServiceMock.Object, routineExerciseService.Object);
        var result = await routineService.AddRoutineExerciseAsync(new RoutineExerciseDto(), routineId, userId);

        Assert.False(result.IsOk);
        Assert.Equal(ErrorCode.NotFound, result.Code);
    }
    
    [Fact]
    public async Task Should_ReturnDataConflictError_When_SentDataAlmostExistsInDatabase()
    {
        int authorId = 10;
        int exerciseId = 11;
        int routineId = 15;
        await SeedUserAsync(authorId);
        var seedExerciseResult = await SeedExerciseAsync(exerciseId, authorId);
        await SeedRoutineAsync(routineId, authorId);

        var exerciseServiceMock = new Mock<IExerciseService>();
        exerciseServiceMock.Setup(e => e.FindExerciseById(It.IsAny<int>())).Returns(Task.FromResult(seedExerciseResult)!);
        var userServiceMock = new Mock<IUserService>();
        var routineExerciseService = new Mock<IRoutineExerciseService>();
        var routineService = new RoutineService(Handler.Context!,exerciseServiceMock.Object, userServiceMock.Object, routineExerciseService.Object);


        var firstSave = await routineService.AddRoutineExerciseAsync(new RoutineExerciseDto{ExerciseId = exerciseId},routineId, authorId);
        var secondFailureSave = await routineService.AddRoutineExerciseAsync(new RoutineExerciseDto{ExerciseId = exerciseId},routineId, authorId);
        Assert.False(secondFailureSave.IsOk);
        Assert.Equal(ErrorCode.DataConflict, secondFailureSave.Code);
    }
    
    [Fact]
    public async Task Should_ReturnDatabaseFailure_When_ErrorsOcorrsWhileSaving()
    {
        int authorId = 10;
        int exerciseId = 11;
        int routineId = 15;
        await SeedUserAsync(authorId);
        var seedExerciseResult = await SeedExerciseAsync(exerciseId, authorId);
        await SeedRoutineAsync(routineId, authorId);

        var exerciseServiceMock = new Mock<IExerciseService>();
        exerciseServiceMock.Setup(e => e.FindExerciseById(It.IsAny<int>())).Returns(Task.FromResult(seedExerciseResult)!);
        var userServiceMock = new Mock<IUserService>();
        var fakeContext = new FailMyContext(Handler.Options!);
        var routineExerciseService = new Mock<IRoutineExerciseService>();
        var routineService = new RoutineService(fakeContext,exerciseServiceMock.Object, userServiceMock.Object, routineExerciseService.Object);


        var result = await routineService.AddRoutineExerciseAsync(new RoutineExerciseDto{ExerciseId = exerciseId},routineId, authorId);
        Assert.False(result.IsOk);
        Assert.Equal(ErrorCode.DatabaseSaveFailure, result.Code);
    }

    [Fact]
    public async Task Should_ReturnOk_When_RoutineExerciseHasBeenSavedInDatabase()
    {
        int authorId = 10;
        int exerciseId = 11;
        int routineId = 15;
        await SeedUserAsync(authorId);
        var seedExerciseResult = await SeedExerciseAsync(exerciseId, authorId);
        await SeedRoutineAsync(routineId, authorId);

        var exerciseServiceMock = new Mock<IExerciseService>();
        exerciseServiceMock.Setup(e => e.FindExerciseById(It.IsAny<int>())).Returns(Task.FromResult(seedExerciseResult)!);
        var userServiceMock = new Mock<IUserService>();
        var routineExerciseService = new Mock<IRoutineExerciseService>();
        var routineService = new RoutineService(Handler.Context!,exerciseServiceMock.Object, userServiceMock.Object, routineExerciseService.Object);


        var result = await routineService.AddRoutineExerciseAsync(new RoutineExerciseDto{ExerciseId = exerciseId},routineId, authorId);
        Assert.True(result.IsOk);
        Assert.Equal(ErrorCode.Ok, result.Code);
    }
}
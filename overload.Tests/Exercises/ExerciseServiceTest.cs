using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using overload.Data;
using overload.DTOs;
using overload.DTOs.Exercises;
using overload.Entities;
using overload.Entities.Enums;
using overload.Fixtures;
using overload.Services;
using overload.Services.Shared;
using overload.Tests.TestUtils;
namespace overload.Tests.Exercises;

[Collection("Integration Test")]
public class ExerciseServiceTest : IAsyncLifetime
{
    public IContextHandler Handler{get;set;}

    public ExerciseServiceTest(TestFixture fixture){
        var scope = fixture.Provider.CreateAsyncScope();
        Handler = scope.ServiceProvider.GetRequiredService<IContextHandler>();
    }
    public async Task InitializeAsync()
    {
        await Handler.InitializeContextAsync();
    }
    public async Task DisposeAsync()
    {
        await Handler.DisposeAsync();
    }
    [Fact]
    public async Task Should_CreateExercise_When_UserExist()
    {
        const int USER_ID = 1;
        Mock<IUserService> userServiceMock = new();
        userServiceMock.Setup(e => e.FindUserById(1)).Returns(new User
        {
            Id = 1,
            FirstName = "Test",
            LastName = "User",
            Username = "testuser",
            Email = "test@email.com",
            Password = "11111111",
            BirthDate = new DateOnly(2000, 1, 1)
        });

        ExerciseService service = new(Handler.Context!, userServiceMock.Object);

        var exerciseDto = new ExerciseDto
        {
            Name = "Leg Pres",
            Muscle = TargetMuscle.Chest,
            Type = ExerciseType.FreeWeight,
            IsPublic = true
        };
        var result = await service.CreateAsync(USER_ID, exerciseDto);
        Assert.True(result.IsOk, $"The message of the error is: {result.Message}");
    }

    [Fact]
    public async Task Should_FailCreateExercise_When_UserIdWasNotFoundInDatabase()
    {
        var dto = new ExerciseDto{};
        var userService = new Mock<IUserService>();
        userService.Setup(e => e.FindUserById(It.IsAny<int>())).Returns((User?)null);

        ExerciseService service = new ExerciseService(Handler!.Context!,userService.Object);
        var result = await service.CreateAsync(0, dto);

        Assert.Equal(ErrorCode.BadCredentials,result.Code);
        Assert.False(result.IsOk);
    }

    [Fact]
    public async Task Should_FailCreateExercise_When_ExerciseNameIsUnavailable()
    {
        const int userId = 1;
        const int exeId = 5;
        var userService = new Mock<IUserService>();
        var exService = new ExerciseService(Handler.Context!, userService.Object);

        var user = await CreateUserHelperAsync(userId);
        var ex = await CreateExerciseHelperAsync(exeId, user.Id);
        var exerciseDto = new ExerciseDto{
            Name = ex.Name, Muscle = ex.Muscle, Type = ex.Type, IsPublic = ex.IsPublic
        };
        var createResult = await exService.CreateAsync(userId, exerciseDto);
        Assert.False(createResult.IsOk);
    }

    [Fact]
    public async Task Should_UpdateExercise_When_UserIsTheAuthor()
    {
        const int USER_ID = 1;
        const int EXERC_ID = 1;
        Mock<IUserService> userServiceMock = new ();
        ExerciseService exercise = new (Handler.Context!, userServiceMock.Object);
        var author = await CreateUserHelperAsync(USER_ID); // off course a exercise can never exists before a user, so this method must run BEFORE the bellow
        var exerciseItem = await CreateExerciseHelperAsync(USER_ID, EXERC_ID);

        ExercisePatchDto updatedExercise = new()
        {
            Name = "Supino Reto com Halteres",
            Muscle = TargetMuscle.Chest, 
            Type = ExerciseType.Bodyweight, 
            IsPublic = true
        };
        Handler.Context!.ChangeTracker.Clear();
        var result = await exercise.UpdateAsync(exerciseItem.Id, updatedExercise,author.Id);
        Assert.True(result.IsOk, $"The error follows: {result.Message}");
    }
    [Fact]
    public async Task Shoud_FailUpdateExercise_When_UserIsNotTheAuthor()
    {
        const int USER_ID_ORIGINAL = 1;
        const int EXERC_ID = 20; // diferent id of the author
        const int USER_IMPOSTOR = 30;
        var userServiceMock = new Mock<IUserService>();
        ExerciseService exerciseService = new (Handler.Context!, userServiceMock.Object);
        await CreateUserHelperAsync(USER_ID_ORIGINAL); // off course a exercise can never exists before a user, so this method must run BEFORE the bellow
        await CreateUserHelperAsync(USER_IMPOSTOR); 
        
        await CreateExerciseHelperAsync(EXERC_ID, USER_ID_ORIGINAL); // this exercise belong to the author with the id number of 20

        ExercisePatchDto updatedExercise = new()
        {
            Name = "chest press",
            Muscle = TargetMuscle.Chest, 
            Type = ExerciseType.Bodyweight, 
            IsPublic = true
        };
        Handler.Context!.ChangeTracker.Clear();
        var result = await exerciseService.UpdateAsync(EXERC_ID, updatedExercise,USER_IMPOSTOR);
        Assert.False(result.IsOk, $"The error follows: {result.Message}");
    }
    [Fact]
    public async Task Should_FailUpdate_When_ExerciseWasNotFound()
    {
        var mockUserService = new Mock<IUserService>();
        ExerciseService exerciseService = new ExerciseService(Handler.Context!, mockUserService.Object);
        var result = await exerciseService.UpdateAsync(1, new ExercisePatchDto{}, 1);
        Assert.False(result.IsOk);
    }
    
    private async Task<User> CreateUserHelperAsync(int id)
    {
        var user = new User 
        { 
            Id = id,
            FirstName = "Test",
            LastName = "User",
            Username = "testuser",
            Email = "test@email.com",
            Password = "11111111",
            BirthDate = new DateOnly(2000, 1, 1)
        };
        Handler.Context!.users.Add(user);
        await Handler.Context.SaveChangesAsync();
        return user;
    }
    private async Task<Exercise> CreateExerciseHelperAsync(int exerciseId, int authorId)
    {
        Exercise exercise = new()
        {
            Id = exerciseId,
            Name = "Biceps",
            Muscle = TargetMuscle.Back,
            Type = ExerciseType.Cable,
            AuthorId = authorId,
            IsPublic = false
        };
        Handler.Context!.exercises.Add(exercise);
        await Handler.Context.SaveChangesAsync();
        return exercise;
    }

    [Fact]
    public async Task Should_ReturnOnlyUserExercises_When_UserIdIsProvided()
    {
        const int firstUserId = 10;
        const int secondUserId = 25;
        const int firstExId = 5;
        const int secondExId = 7;

        var userService = new Mock<IUserService>();
        var exerciseService = new ExerciseService(Handler.Context!, userService.Object);

        var fisrtUser = CreateUserHelperAsync(firstUserId);
        var secondUser = CreateUserHelperAsync(secondUserId);
        var firstExercise = CreateExerciseHelperAsync(firstExId, firstUserId);
        var secondExercise = CreateExerciseHelperAsync(secondExId, secondUserId);

        var result = await exerciseService.GetAllAsync(10);
        Assert.True(result.Count == 1);
    }
}

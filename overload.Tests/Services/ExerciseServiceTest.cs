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
using overload.Tests.Common;
using overload.Tests.Data;
namespace overload.Tests.Services;

[Collection("Integration Test")]
public class ExerciseServiceTest : IntegrationTestBase
{
    public ExerciseServiceTest(TestFixture Fixture) : base (Fixture){}
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

        var user = await SeedUserAsync(userId);
        var ex = await SeedExerciseAsync(exeId, user.Id);
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
        var author = await SeedUserAsync(USER_ID); // off course a exercise can never exists before a user, so this method must run BEFORE the bellow
        var exerciseItem = await SeedExerciseAsync(USER_ID, EXERC_ID);

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
        await SeedUserAsync(USER_ID_ORIGINAL); // off course a exercise can never exists before a user, so this method must run BEFORE the bellow
        await SeedUserAsync(USER_IMPOSTOR); 
        
        await SeedExerciseAsync(EXERC_ID, USER_ID_ORIGINAL); // this exercise belong to the author with the id number of 20

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

    [Fact]
    public async Task Should_ReturnOnlyUserExercises_When_UserIdIsProvided()
    {
        const int firstUserId = 10;
        const int secondUserId = 25;
        const int firstExId = 5;
        const int secondExId = 7;

        var userService = new Mock<IUserService>();
        var exerciseService = new ExerciseService(Handler.Context!, userService.Object);

        await SeedUserAsync(firstUserId); // create the first user
        await SeedUserAsync(secondUserId); // create the second user
        await SeedExerciseAsync(firstExId, firstUserId); // create the first exerc. for first user
        await SeedExerciseAsync(secondExId, secondUserId); // create the second exerc. for second user

        var result = await exerciseService.GetAllAsync(10);
        Assert.Single(result);
    }
}

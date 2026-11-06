using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using overload.Data;
using overload.DTOs;
using overload.DTOs.Exercises;
using overload.Entities;
using overload.Entities.Enums;
using overload.Services;
namespace overload.Tests.Exercises;
public class ExerciseServiceTest : IAsyncLifetime
{
    public SqliteConnection _conn = default!;
    public MyContext ctx {get;set;} = default!;
    public async Task InitializeAsync()
    {
        _conn = new SqliteConnection("Data Source=:memory:");
        await _conn.OpenAsync();
        var opt = new DbContextOptionsBuilder<MyContext>().UseSqlite(_conn).Options;
        ctx = new MyContext(opt);
        await ctx.Database.EnsureCreatedAsync();
    }
    public async Task DisposeAsync()
    {
        if(ctx != null) await ctx.DisposeAsync();
        if(_conn != null) await _conn.DisposeAsync();
    }
    [Fact]
    public async Task MustCreateExerciseWhenTheUserExist()
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

        ExerciseService service = new(ctx, userServiceMock.Object);

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
    public async Task Should_UpdateExercise_When_UserIsTheAuthor()
    {
        const int USER_ID = 1;
        const int EXERC_ID = 1;
        Mock<IUserService> userServiceMock = new ();
        ExerciseService exercise = new (ctx, userServiceMock.Object);
        await CreateUserHelper(EXERC_ID); // off course a exercise can never exists before a user, so this method must run BEFORE the bellow
        await CreateExerciseHelper(USER_ID, EXERC_ID);

        ExercisePatchDto updatedExercise = new()
        {
            Name = "Supino Reto com Halteres",
            Muscle = TargetMuscle.Chest, 
            Type = ExerciseType.Bodyweight, 
            IsPublic = true
        };
        var result = await exercise.Update(EXERC_ID, updatedExercise,USER_ID);
        Assert.True(result.IsOk, $"The error follows: {result.Message}");
    }
    [Fact]
    public async Task Shoud_FailUpdateExercise_When_UserIsNotTheAuthor()
    {
        const int USER_ID_ORIGINAL = 1;
        const int EXERC_ID = 20; // diferent id of the author
        const int USER_ID_IMPOSTOR = 30;
        var userServiceMock = new Mock<IUserService>();
        ExerciseService exerciseService = new (ctx, userServiceMock.Object);
        await CreateUserHelper(USER_ID_ORIGINAL); // off course a exercise can never exists before a user, so this method must run BEFORE the bellow
        await CreateUserHelper(USER_ID_IMPOSTOR); 
        
        await CreateExerciseHelper(EXERC_ID, USER_ID_ORIGINAL); // this exercise belong to the author with the id number of 20

        ExercisePatchDto updatedExercise = new()
        {
            Name = "Supino Reto com Halteres",
            Muscle = TargetMuscle.Chest, 
            Type = ExerciseType.Bodyweight, 
            IsPublic = true
        };
        var result = await exerciseService.Update(EXERC_ID, updatedExercise,USER_ID_IMPOSTOR);
        Assert.False(result.IsOk, $"The error follows: {result.Message}");
    }
    public async Task<User> CreateUserHelper(int id)
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
        ctx.users.Add(user);
        await ctx.SaveChangesAsync();
        return user;
    }
    public async Task<Exercise> CreateExerciseHelper(int exerciseId, int authorId)
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
        ctx.exercises.Add(exercise);
        await ctx.SaveChangesAsync();
        return exercise;
    }
}

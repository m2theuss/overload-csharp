using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using overload.Data;
using overload.DTOs;
using overload.Entities;
using overload.Entities.Enums;
using overload.Services;
namespace overload.Tests.Exercises;
public class DbExerciseFixture : IAsyncLifetime
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
}
public class ExerciseServiceTest : IClassFixture<DbExerciseFixture>
{
    private readonly DbExerciseFixture _fixture;
    public ExerciseServiceTest(DbExerciseFixture dbFixture)
    {
        _fixture = dbFixture;
    }
    [Fact]
    public async Task MustCreateExerciseWhenTheDataIsValid()
    {
        Mock<IUserService> userServiceMock = new();
        userServiceMock.Setup(e => e.FindUserById(1)).Returns(new User{Id = 1,Password="11111111"});
        ExerciseService service = new(_fixture.ctx, userServiceMock.Object);
        var a = userServiceMock.Object;

        var exerciseDto = new ExerciseDto
        {
            Name = "Supino Reto com Halteres",
            Muscle = TargetMuscle.Chest,
            Type = ExerciseType.FreeWeight,
            IsPublic = true
        };
        var result = await service.CreateAsync(1, exerciseDto);
        Assert.True(result.IsOk);
    }
}

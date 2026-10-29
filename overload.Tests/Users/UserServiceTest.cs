using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
using overload.Services;
namespace overload.Tests.Users;
public class DbUserFixture : IAsyncLifetime
{
    private SqliteConnection _conn = default!;
    public MyContext ctx{get; private set;} = default!;
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
        if(_conn != null) _conn.Dispose();
        if(ctx != null) ctx.Dispose();
    }
    
}
public class UserServiceTest : IClassFixture<DbUserFixture>
{
    private readonly DbUserFixture _fixture;

    public UserServiceTest(DbUserFixture dbFixture)
    {
        _fixture = dbFixture;
    }
    [Fact]
    public async Task MustCreateUserWhenDataValid()
    {
        UserService service = new(_fixture.ctx);
        User user = new()
        {
            FirstName = "Joshn",
            LastName = "Dsoe",
            Username = "johsndoe",
            Email = "johndoes@email.com",
            Password = "Password123!",
            BirthDate = new DateOnly(1995, 5, 15)
        };

        var result = await service.createAsync(user);

        Assert.True(result.IsOk);
    }

    [Fact]
    public async Task DontMustCreateUserWhenDataIsTheSame()
    {
        UserService service = new(_fixture.ctx);
        User user = new()
        {
            FirstName = "Joshn",
            LastName = "Dsoe",
            Username = "johsndoe",
            Email = "johndoes@email.com",
            Password = "Password123!",
            BirthDate = new DateOnly(1995, 5, 15)
        };
        var result = await service.createAsync(user);
        Assert.False(result.IsOk);
    }
}
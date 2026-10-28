using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
using overload.Services;

public class UserServiceTest
{
    [Fact]
    public async Task MustCreateUserWhenDataValid()
    {
        await using var conn = new SqliteConnection("Data source=memory:");
        await conn.OpenAsync();
        var opt = new DbContextOptionsBuilder<MyContext>().UseSqlite(conn).Options; 
        await using var ctx = new MyContext(opt);
        await ctx.Database.EnsureCreatedAsync();

        UserService service = new UserService(ctx);
        User user = new User
        {
             FirstName = "Joshn",
            LastName = "Dsoe",
            Username = "johsndoe",
            Email = "johndoes@email.com",
            Password = "Password123!",
            BirthDate = new DateOnly(1995, 5, 15)
        };

        var result = await service.createAsync(user);

        Assert.Equal("Valid",result.Data);
    }
}
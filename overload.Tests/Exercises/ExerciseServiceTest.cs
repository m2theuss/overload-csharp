using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using overload.Data;

public class DbExerciseFixture : IAsyncLifetime
{
    private SqliteConnection _conn = default!;
    private DbContext ctx = default!;
    public async Task InitializeAsync()
    {
        _conn = new SqliteConnection("Data Source=:memory:");
        await _conn.OpenAsync();
        var opt = new DbContextOptionsBuilder<MyContext>().UseSqlite().Options;
        ctx = new DbContext(opt);
    }
    public async Task DisposeAsync()
    {
        if(ctx != null) ctx.Dispose();
        if(_conn != null) _conn.Dispose();
    }
}
public class ExerciseServiceTest
{
    [Fact]
    public Task MustCreateExerciseWhenTheDataIsValid()
    {
        
    }
}

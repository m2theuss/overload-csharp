using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using overload.Data;
namespace overload.Tests.TestUtils;
//this class handle of conection with SQLITE and return a DatabaseInfor, witch represents a conection and a context

public interface IContextHandler
{
    public Task<DatabaseInfo> InitializeContextAsync(); 
}
public class ContextHandler : IContextHandler
{
    public async Task<DatabaseInfo> InitializeContextAsync(){
        var conn = new SqliteConnection("Data Source=:memory:");
        await conn.OpenAsync();
        var opt = new DbContextOptionsBuilder<MyContext>().UseSqlite(conn).Options;
        var ctx = new MyContext(opt);
        await ctx.Database.EnsureCreatedAsync();
        return new DatabaseInfo{
            conection = conn,
            context = ctx 
        };
    }    
}
public class DatabaseInfo : IAsyncDisposable
{
    public SqliteConnection? Conection{get;set;}
    public MyContext? Context{get;set;}

    public async ValueTask DisposeAsync()
    {
         Conection?.DisposeAsync();
        await Context.DisposeAsync();
    }
}
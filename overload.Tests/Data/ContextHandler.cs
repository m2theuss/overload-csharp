using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using overload.Data;
namespace overload.Tests.Data;
//this class handle of conection with SQLITE and return a DatabaseInfor, witch represents a conection and a context

public interface IContextHandler
{
    public SqliteConnection? Connection{get;set;}
    public MyContext? Context{get;set;}
    public DbContextOptions<MyContext>? Options{get;set;}
    public Task InitializeContextAsync(); 
    public ValueTask DisposeAsync();
}
public class ContextHandler : IContextHandler, IAsyncDisposable
{
    public SqliteConnection? Connection{get;set;}
    public MyContext? Context{get;set;}
    public DbContextOptions<MyContext>? Options{get;set;}

    public async Task InitializeContextAsync(){
        Connection = new SqliteConnection("Data Source=:memory:");
        await Connection.OpenAsync();
        var opt = new DbContextOptionsBuilder<MyContext>().UseSqlite(Connection).Options;
        Options = opt;
        Context = new MyContext(opt);
        await Context.Database.EnsureCreatedAsync();
    }
    public async ValueTask DisposeAsync()
    {
        if(Connection!= null) await Connection!.DisposeAsync();
        if(Context != null) await Context!.DisposeAsync();
        GC.SuppressFinalize(this); //hence the object almost was released above, just tell it to the c# GC
    }
}

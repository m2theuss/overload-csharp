using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using overload.Data;

namespace overload.Tests.Data;
public class FailMyContext : MyContext
{
    public FailMyContext(DbContextOptions<MyContext> options)
        : base(options){}

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0); // this act like a failed save changes. bc when this works, that return > 0 
    }
}
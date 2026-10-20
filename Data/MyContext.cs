using System.Reflection;
using Microsoft.EntityFrameworkCore;
using overload.Entities;
namespace overload.Data;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users{get; set;}
    public DbSet<Exercise> Exercises{get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
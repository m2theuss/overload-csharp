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
    public DbSet<User> users{get; set;}
    public DbSet<Exercise> exercises{get; set;}
    public DbSet<Routine> routines{get;set;}
    public DbSet<RoutineExercise> routine_exercises{get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
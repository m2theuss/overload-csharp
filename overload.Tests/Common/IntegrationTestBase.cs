using Microsoft.Extensions.DependencyInjection;
using overload.Entities;
using overload.Entities.Enums;
using overload.Fixtures;
using overload.Tests.Data;

namespace overload.Tests.Common;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    public IContextHandler Handler{get;set;}
    public AsyncServiceScope _scope;
    
    public IntegrationTestBase(TestFixture fixture)
    {
        _scope = fixture.Provider.CreateAsyncScope();
        Handler = _scope.ServiceProvider.GetRequiredService<IContextHandler>();
    }

    public async Task InitializeAsync()
    {
        await Handler.InitializeContextAsync();
    }

    public async Task DisposeAsync()
    {
        await Handler.DisposeAsync();
        await _scope.DisposeAsync();
    }
    protected async Task<User> SeedUserAsync(int id)
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
        Handler.Context!.users.Add(user);
        await Handler.Context.SaveChangesAsync();
        return user;
    }
    protected User BuildUser(int id)
    {
        return new User
        {
            Id = id,
            FirstName = "Test",
            LastName = "User",
            Username = "testuser",
            Email = "test@email.com",
            Password = "11111111",
            BirthDate = new DateOnly(2000, 1, 1)
        };
    }

    protected async Task<Exercise> SeedExerciseAsync(int exerciseId, int authorId)
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
        Handler.Context!.exercises.Add(exercise);
        await Handler.Context.SaveChangesAsync();
        return exercise;
    }
    
    protected Exercise BuildExercise(int exerciseId, int authorId)
    {
        return new Exercise
        {
            Id = exerciseId,
            Name = "Biceps",
            Muscle = TargetMuscle.Back,
            Type = ExerciseType.Cable,
            AuthorId = authorId,
            IsPublic = false
        };
    }
}
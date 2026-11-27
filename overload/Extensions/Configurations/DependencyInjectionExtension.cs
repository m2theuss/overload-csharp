using overload.Services;
using overload.Utils;

namespace overload.Extensions.Configurations;

public static class DependencyInjectionExtensions 
{
    public static IServiceCollection AddDependencies(this IServiceCollection service)
    {
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IExerciseService, ExerciseService>();
        service.AddScoped<IRoutineService, RoutineService>();
        service.AddScoped<IRoutineExerciseService, RoutineExerciseService>();
        service.AddScoped<IJwtUtils,JwtUtils>();
        return service;
    }
}
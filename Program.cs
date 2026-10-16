using Microsoft.EntityFrameworkCore;
using overload.Common.Converters;
using overload.Data;
using overload.Exceptions;
using overload.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // 1. Tell ASP.NET Core NOT to automatically send a 400 response
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new SafeDateOnlyConverter());
    });
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<MyContext>(options =>
    options.UseNpgsql(connectionString)
);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();
app.UseExceptionHandler();
app.MapControllers();
app.Run();

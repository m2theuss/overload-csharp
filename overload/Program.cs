using Microsoft.EntityFrameworkCore;
using overload.Common.Converters;
using overload.Data;
using overload.Exceptions;
using System.Text.Json.Serialization;
using overload.Extensions.Configurations;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.AddJwtAuthentication();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // 1. Tell ASP.NET Core NOT to automatically send a 400 response
        options.SuppressModelStateInvalidFilter = false;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new SafeDateOnlyConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<MyContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDependencies();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyContext>();
    dbContext.Database.Migrate();
}
app.UseAuthentication();

app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.Run();

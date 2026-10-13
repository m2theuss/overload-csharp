using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<MyContext>(options =>
    options.UseNpgsql(connectionString)
);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();
app.MapControllers();
app.Run();

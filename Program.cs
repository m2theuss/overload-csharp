using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<MyContext>(options =>
    options.UseOracle(connectionString)
);
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();
app.MapControllers();
app.Run();

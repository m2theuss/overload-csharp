using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace overload.Extensions.Configurations;

public static class AuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this WebApplicationBuilder builder){
        builder.Services.AddAuthentication(opt =>{
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt => {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["AuthConfiguration:Issuer"],
                ValidAudience = builder.Configuration["AuthConfiguration:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["AuthConfiguration:Key"]!))
            };
        });
        return builder.Services;
    }
}
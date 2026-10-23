namespace overload.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using overload.Entities;

public class JwtUtils(IConfiguration _configuration){
    public string generateJwtToken(User user){
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["AuthConfiguration:Key"]!);
        var descriptor = new SecurityTokenDescriptor{ 
            Subject = new ClaimsIdentity(new[] {
                new Claim(
                    "id",user.Id.ToString()
                ),
                new Claim(
                    "username",user.Username
                )
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
    public ClaimsPrincipal? validateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["AuthConfiguration:Key"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out SecurityToken validatedToken
            );

            return principal;
        }
        catch (SecurityTokenException)
        {
            return null;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}
public readonly struct TokenResponse
{
    public string Token{get;init;}
    public string TokenType{get; init;}

    public TokenResponse(string token, string tokenType)
    {
        Token = token;
        TokenType = tokenType;
    }
}


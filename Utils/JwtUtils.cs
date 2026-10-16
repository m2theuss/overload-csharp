namespace overload.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using overload.Entities;

public class JwtUtils{
    public static string generateJwtToken(User user){
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("mypasswordverysafetyverygood");
        var descriptor = new SecurityTokenDescriptor{ 
            Subject = new ClaimsIdentity(new[] {new Claim("id",user.Id.ToString())}),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
}
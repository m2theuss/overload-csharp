using System.Security.Claims;

namespace overload.Extensions;

public static class ClaimsPrincipalExtension
{
    public static int GetUserIdFromToken(this ClaimsPrincipal user)
    {
        string? strId = user.FindFirstValue("id");
        if(!int.TryParse(strId, out int result))
        {
           throw new InvalidOperationException("Invalid token or user id unavailable"); 
        }
        return result;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace overload.Entities;

public class User
{
    public int Id {get; set;}
 
    [Required(ErrorMessage = "First name must be filled")]
    public String FirstName { get; set; } = String.Empty;

    [Required(ErrorMessage = "Last name must be filled")]
    public String LastName { get; set; } = String.Empty;

    [Required(ErrorMessage = "Username must be filled")]
    public String Username { get; set; } = String.Empty;

    [Required(ErrorMessage = "Email must be filled")]
    public String Email { get; set; } = String.Empty;

    [Required(ErrorMessage = "Password must be filled")]
    [StringLength(64, MinimumLength = 8, ErrorMessage = "Password must be beetween 8 and 64 characters")]
    public String Password { get; set; } = String.Empty;

    [Required(ErrorMessage = "Birth date must be filled")]
    public DateOnly BirthDate { get; set; }
    public DateTime CreatedAt { get; init;} = DateTime.Now.ToUniversalTime();
}
using System.ComponentModel.DataAnnotations;

namespace overload.DTOs;
public class UserDto{
    public string Username {get; set;} = String.Empty;
    public string Email {get; set;} = String.Empty;
    
    [Required(ErrorMessage = "A password must be provide")]
    public string Password {get; set;} = String.Empty;
}
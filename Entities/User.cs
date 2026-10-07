namespace overload.Entities;

public class User
{
    public int Id {get; set;}
    public String First_name {get; set;} = String.Empty;
    public String Last_name {get; set;} = String.Empty;
    public String Username {get; set;} = String.Empty;
    public String Email {get; set;} = String.Empty;
    public String Password {get; set;} = String.Empty;
    public DateOnly Birthdate {get; set;}
    
}
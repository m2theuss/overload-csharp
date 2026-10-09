using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace overload.Entities;

public class User
{
    [JsonIgnore]
    private DateOnly _birthDate;
    public int Id {get; set;}
    public String First_name {get; set;} = String.Empty;
    public String Last_name {get; set;} = String.Empty;
    public String Username {get; set;} = String.Empty;
    public String Email {get; set;} = String.Empty;
    public String Password {get; set;} = String.Empty;

    [NotMapped]
    [JsonPropertyName("BirthDate")]
    public String TextBirthDate
    {
        
    }
    
}
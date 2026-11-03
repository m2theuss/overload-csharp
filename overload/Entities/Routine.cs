namespace overload.Entities;

public class Routine
{
    public int Id{get; set;}
    public string? Name{get;set;}
    public DayOfWeek Day{get;set;}
    public bool IsPublic {get; set;}
    public int AuthorId{get;set;}
    public User Author{get;set;}
    public DateTime CreatedAt{get; init;} = DateTime.Now.ToUniversalTime();

}
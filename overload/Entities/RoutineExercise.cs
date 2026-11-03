using overload.Entities;

namespace overload.Data;

public class RoutineExercise
{
    public int Id{get;set;}
    public int Order{get;set;}
    public int RoutineId{get;set;}
    public Routine Routine{get;set;}
    public int ExerciseId{get;set;}
    public Exercise ExerciseItem{get;set;}
    public DateTime CreatedAt{get;set;} = DateTime.Now.ToUniversalTime();
}
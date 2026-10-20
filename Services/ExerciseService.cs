using overload.Data;
using overload.DTOs;
public interface ExerciceService
{
    public void create(string id, ExerciseDto dto);
}
public class ExerciseService(MyContext _myContext)
{
    public void create(string id, ExerciseDto dto)
    {
        _myContext.Add()
    }
}
using Microsoft.EntityFrameworkCore;
using overload.Data;
using overload.Entities;
namespace overload.Services;

public interface IUserService
{
    public Task create(User user);
}
public class UserService : IUserService
{  
    private MyContext _myContext;
    public UserService(MyContext myContext)
    {
        this._myContext = myContext;
    }
    public async Task create(User user)
    {
        _myContext.Add<User>(user);
        await _myContext.SaveChangesAsync();
    }
}
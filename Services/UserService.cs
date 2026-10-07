using overload.Entities;

namespace overload.Services;

public interface IUserService
{
    public Boolean create(User user);
}
public class UserService : IUserService
{
    public Boolean create(User user)
    {
        return true;
    }
}
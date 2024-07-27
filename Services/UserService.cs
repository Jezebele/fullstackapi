using FullStackAPI.Data;
using FullStackAPI.Models;

public interface IUserService
{
    User GetUserById(int userId);
    bool CheckPassword(User user, string password);
}

public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    public User GetUserById(int userId)
    {
        return _context.Users.Find(userId);
    }

    public bool CheckPassword(User user, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }
}

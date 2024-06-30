using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User Login(string email, string password);
        bool PostUser(User user);
        bool PutUser(int id, User user);
    }
}
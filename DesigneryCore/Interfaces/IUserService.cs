using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
    }
}
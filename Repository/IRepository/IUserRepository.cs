using Be_My_Voice_Backend.Models;

namespace Be_My_Voice_Backend.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel[]> getAllUsers();
    }
}

using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;

namespace Be_My_Voice_Backend.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel[]> getAllUsers();
        bool isUniqueUser(string email);
        UserModel getUser(Guid userId);
        Task<UserModel> isUserAuthenticated(string email, string password);
        Task<UserModel> registerUser(RegisterRequestDTO registerRequestDTO);
    }
}

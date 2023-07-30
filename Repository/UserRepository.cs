using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Be_My_Voice_Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public UserRepository(ApplicationDBContext dbCotetxt)
        {
            _dbContext = dbCotetxt;
        }

        public async Task<UserModel[]> getAllUsers()
        {
           return await _dbContext.users.ToArrayAsync();
        }
    }
}

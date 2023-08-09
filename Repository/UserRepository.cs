using BCrypt.Net;
using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Be_My_Voice_Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly string secretKey;
        private readonly string passwordSalt;

        public UserRepository(ApplicationDBContext dbCotetxt, IConfiguration configuration)
        {
            _dbContext = dbCotetxt;
            secretKey = configuration.GetSection("AppSettings:Secret").Value;
            passwordSalt = configuration.GetSection("AppSettings:PasswordSalt").Value;
        }

        public async Task<UserModel[]> getAllUsers()
        {
            return await _dbContext.users.ToArrayAsync();
        }

        public UserModel getUser(Guid userId)
        {
            UserModel user = null;
            try
            {
                user = _dbContext.users.FirstOrDefault(u => u.UserID == userId);
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        public UserModel getUser(string email)
        {
            UserModel user = null;
            try
            {
                user = _dbContext.users.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        public bool isUniqueUser(string email)
        {
            UserModel user = null;
            try
            {
                user = _dbContext.users.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception)
            {
                throw;
            }

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<UserModel> isUserAuthenticated(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> registerUser(RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                UserModel user = new UserModel()
                {
                    UserID = Guid.NewGuid(),
                    Name = registerRequestDTO.Name,
                    Email = registerRequestDTO.Email,
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerRequestDTO.Password),
                    Role = registerRequestDTO.Role,
                    Status = registerRequestDTO.Status,
                    ProfilePictureUrl = registerRequestDTO.ProfilePictureUrl,
                    PhoneNumber = registerRequestDTO.PhoneNumber,
                    DateOfBirth = registerRequestDTO.DateOfBirth    
                };

                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

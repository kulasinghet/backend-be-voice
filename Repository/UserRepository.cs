using BCrypt.Net;
using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<UserModel> getUserById(Guid userId)
        {
            UserModel user = null;
            try
            {
                user = await _dbContext.users.FirstOrDefaultAsync(u => u.UserID == userId);
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

        public async Task<UserModel> isUserAuthenticated(string email, string password)
        {
            return await _dbContext.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserModel> registerUser(RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                UserModel user = new()
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

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Email == loginRequestDTO.Email);

            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    RequestId = "User not found",
                    User = null,
                    Token = null
                };
            }

            var isPasswordMatch = BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.PasswordHash, true);

            if (isPasswordMatch)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user.UserID.ToString()),
                                new Claim(ClaimTypes.Role, user.Role)
                            }
                        ),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponseDTO loginResponseDTO = new()
                {
                    Token = tokenHandler.WriteToken(token),
                    User = user
                };

                return loginResponseDTO;

            }
            else
            {
                return new LoginResponseDTO()
                {
                    RequestId = "Password is incorrect",
                    User = null,
                    Token = null
                };
            }
        }
    }
}

using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Be_My_Voice_Backend.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }


        [HttpGet("get-all-users")]
        public async Task<ActionResult<APIResponse>> GetAllUsers()
        {

            try
            {
                UserModel[] users = await _userRepository.getAllUsers();
                return (new APIResponse(200, true, "All users in the DB", users));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }

        }

        [HttpGet("get-user-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetUserById(Guid id)
        {
            try
            {
                UserModel user = await _userRepository.getUserById(id);
                return (new APIResponse(200, true, "User found", user));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> RegisterUser(RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return (new APIResponse(400, false, "Invalid Model State", ModelState));
                }
                if (!_userRepository.isUniqueUser(registerRequestDTO.Email))
                {
                    return (new APIResponse(400, false, "User already exists"));
                }
                UserModel user = await _userRepository.registerUser(registerRequestDTO);
                return (new APIResponse(201, true, "User created", user));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return (new APIResponse(400, false, "Invalid Model State", ModelState));
                }

                LoginResponseDTO loginResponse = await _userRepository.Login(loginRequest);
                
                if (loginResponse.Token == null)
                {
                    return (new APIResponse(400, false, "Invalid Credentials"));
                }
                
                return (new APIResponse(200, true, "User logged in", loginResponse));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.ToString()));
            }   

        }
    }
}

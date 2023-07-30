using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Be_My_Voice_Backend.Controllers
{
    [Route("api/session")]
    public class SessionController : Controller
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IUserRepository _userRepository;

        public SessionController(ISessionsRepository sessionsRepository, IUserRepository userRepository)
        {
            this._sessionsRepository = sessionsRepository;
            this._userRepository = userRepository;
        }

        [HttpGet("get-all-sessions")]
        public async Task<ActionResult<APIResponse>> getAllSessions()
        {
            try
            {
                SessionModel[] sessions = await _sessionsRepository.getAllSessions();
                return (new APIResponse(200, true, "All sessions in the DB", sessions));
            } catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpGet("get-session-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> getSessionById(Guid id)
        {
            try
            {
                SessionModel session = await _sessionsRepository.getSessionById(id);
                return (new APIResponse(200, true, "Session found", session));
            } catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpGet("get-sessions-by-user-id/{id}")]
        public async Task<ActionResult<APIResponse>> getSessionsByUserId(Guid id)
        {
            try
            {
                SessionModel[] sessions = await _sessionsRepository.getSessionsByUserId(id);
                return (new APIResponse(200, true, "Sessions found", sessions));
            } catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpPost("create-session")]
        public async Task<ActionResult<APIResponse>> createSession([FromBody] CreateSessionDTO createSessionDTO) 
        {
            try
            {
                SessionModel session = new SessionModel();
                session.userID = createSessionDTO.userID;
                session.sessionID = Guid.NewGuid();
                session.startDtae = DateTime.Now;
                session.endDate = DateTime.Now.AddMinutes(5);

                Guid userID = Guid.Parse(createSessionDTO.userID.ToString());
                // TODO: implement check if user exists
                //UserModel user = await _userRepository.getUserById(userID);

                //if (user == null)
                //{
                //    return (new APIResponse(404, false, "User not found"));
                //}


                SessionModel newSession = await _sessionsRepository.createSession(session);
                return (new APIResponse(201, true, "Session created", newSession));
            } catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpPatch("update-session")]
        public async Task<ActionResult<APIResponse>> updateSession([FromBody] SessionModel session)
        {
            try
            {
                if (session.sessionID == Guid.Empty)
                {
                    return (new APIResponse(406, false, "Please provide a valid session ID"));
                }

                if (await _sessionsRepository.getSessionById(session.sessionID) == null)
                {
                    return (new APIResponse(204, false, "Session is not found"));
                }

                SessionModel updatedSession = await _sessionsRepository.updateSession(session);

                if (updateSession != null)
                {
                    return (new APIResponse(200, true, "Session updated", updatedSession));
                } else
                {
                    return (new APIResponse(406, false, "Something went wrong while updating"));
                }

            } catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }
    }
}

using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Be_My_Voice_Backend.Controllers
{
    [Route("api/normal-user-translations")]
    public class NormalUserTranslationController : Controller
    {
        private readonly INormalUserTranslationsRepository _normalUserTranslationsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionsRepository _sessionsRepository;

        public NormalUserTranslationController(INormalUserTranslationsRepository normalUserTranslationsRepository, IUserRepository userRepository, ISessionsRepository sessionsRepository)
        {
            this._normalUserTranslationsRepository = normalUserTranslationsRepository;
            this._userRepository = userRepository;
            this._sessionsRepository = sessionsRepository;
        }

        [HttpGet("get-all-normal-user-translations")]
        public async Task<ActionResult<APIResponse>> getAllTranslations()
        {
            try
            {
                NormalUserTranslationModel[] translations = await _normalUserTranslationsRepository.NormalUserTranslationModel();
                return (new APIResponse(200, true, "All normal user translations in the DB", translations));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpGet("get-normal-user-translation-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> getTranlationById(Guid id)
        {
            try
            {
                NormalUserTranslationModel translation = await _normalUserTranslationsRepository.getNormalUserTranslationById(id);
                return (new APIResponse(200, true, "Normal user translation Found", translation));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message, ex.StackTrace));
            }
        }

        [HttpGet("get-normal-user-translation-by-session/{id}")]
        public async Task<ActionResult<APIResponse>> getTranlationBySessionId(Guid id)
        {
            try
            {
                if (await _sessionsRepository.getSessionById(id) == null)
                {
                    return new APIResponse(406, false, "Invalid session id");
                }


                NormalUserTranslationModel[] translations = await _normalUserTranslationsRepository.getNormalUserTranslationBySessionID(id);

                if (translations.Count() == 0)
                {
                    return new APIResponse(200, true, "No translations found for given session id");
                }

                return (new APIResponse(200, true, "Translations Found", translations));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message, ex.StackTrace));
            }
        }

        [HttpPost("create-normal-user-translation")]
        public async Task<ActionResult<APIResponse>> createTranslation([FromBody] CreateTranslationDTO createTranslationDTO, IFormFile voiceRecord)
        {

            try
            {

                String resultObjectToRunOnAIModel = createTranslationDTO.resultObjectFromSkeleton.ToString();

                // TODO: Run inference on the resultObject

                String predictedText = "";


                SessionModel session = await _sessionsRepository.getSessionById(createTranslationDTO.sessionID);

                if (session == null)
                {
                    return new APIResponse(406, false, "Invalid Session id");
                }

                //if (await _userRepository.getUserById(createTranslationDTO.userID) == null)
                //{
                //    return new APIResponse(406, false, "Invalid user id");
                //}

                if (session.startDate.CompareTo(DateTime.Now) > 0)
                {
                    return new APIResponse(406, false, "Session hasn't started yet");
                }

                if (session.status != "ongoing")
                {
                    return new APIResponse(406, false, "Session is not ongoing");
                }

                if (session.endDate.CompareTo(DateTime.Now) < 0)
                {
                    await _sessionsRepository.updateSessionStatus(
                        new UpdateSessionStatusDTO
                        {
                            sessionID = session.sessionID,
                            status = "completed"
                        }
                    );

                    return new APIResponse(406, false, "Session has expired");
                }




                return new APIResponse(201, true, "Translation created successfully");

            } catch (Exception ex)
            {
                return new APIResponse(500, false, ex.Message);
            }

        }

        [HttpDelete("delete-normal-user-transaction-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> deleteTransaction(Guid id)
        {
            if (id == null)
            {
                return new APIResponse(404, false, "Please provide a valid transaction ID");
            }

            NormalUserTranslationModel translationModel = await _normalUserTranslationsRepository.deleteNormalUserTranslationById(id);

            if (translationModel == null)
            {
                return new APIResponse(406, false, "Something went wrong while deleting");
            }

            return new APIResponse(201, true, "Successfully deleted", translationModel);
        }

        [HttpPatch("update-normal-user-translation")]
        public async Task<ActionResult<APIResponse>> updateSession([FromBody] NormalUserTranslationModel translation)
        {
            try
            {
                if (translation.NormalUserTranslationID == Guid.Empty)
                { 
                    return (new APIResponse(406, false, "Please provide a valid translation ID"));
                }

                if (await _sessionsRepository.getSessionById(translation.SessionID) == null)
                {
                    return (new APIResponse(204, false, "Session is not found"));
                }

                NormalUserTranslationModel updatedTranslation = await _normalUserTranslationsRepository.updateNormalUserTranslation(translation);

                if (updatedTranslation != null)
                {
                    return (new APIResponse(200, true, "Translation updated", updatedTranslation));
                }
                else
                {
                    return (new APIResponse(501, false, "Something went wrong while updating"));
                }

            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }
    }
}

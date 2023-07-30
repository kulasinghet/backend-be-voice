using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Be_My_Voice_Backend.Controllers
{
    [Route("api/translation")]
    public class TranslationController : Controller
    {
        private readonly ITranslationsRepository _translationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionsRepository _sessionsRepository;

        public TranslationController(ITranslationsRepository translationRepository, IUserRepository userRepository, ISessionsRepository sessionsRepository)
        {
            this._translationRepository = translationRepository;
            this._userRepository = userRepository;
            this._sessionsRepository = sessionsRepository;
        }

        [HttpGet("get-all-translations")]
        public async Task<ActionResult<APIResponse>> getAllTranslations()
        {
            try
            {
                TranslationModel[] translations = await _translationRepository.getAllTranslations();
                return (new APIResponse(200, true, "All translations in the DB", translations));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }
        }

        [HttpGet("get-translation-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> getTranlationById(Guid id)
        {
            try
            {
                TranslationModel translation = await _translationRepository.getTranslationById(id);
                return (new APIResponse(200, true, "Translation Found", translation));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message, ex.StackTrace));
            }
        }

        [HttpGet("get-translation-by-session/{id}")]
        public async Task<ActionResult<APIResponse>> getTranlationBySessionId(Guid id)
        {
            try
            {
                if (await _sessionsRepository.getSessionById(id) == null)
                {
                    return new APIResponse(406, false, "Invalid session id");
                }


                TranslationModel[] translations = await _translationRepository.getlTranslationBySessionID(id);

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

        [HttpPost("create-translation")]
        public async Task<ActionResult<APIResponse>> createTranslation([FromBody] CreateTranslationDTO createTranslationDTO)
        {
            try
            {

                String resultObjectToRunOnAIModel = createTranslationDTO.resultObjectFromSkeleton;

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

                Console.Out.WriteLine(session.endDate.ToString());
                Console.Out.WriteLine(DateTime.Now.ToString());

                if (session.endDate.CompareTo(DateTime.Now) < 0)
                {
                    return new APIResponse(406, false, "Session has expired");
                }

                TranslationModel translation = new TranslationModel();
                translation.sessionID = session.sessionID;
                translation.translatedText = predictedText;

                //if (translation.translatedText != null)
                //{
                //    return new APIResponse(500, false, "Couldn't predict the video");
                //}

                TranslationModel result = await _translationRepository.createTranslation(translation);

                if (result == null)
                {
                    return new APIResponse(500, false, "Couldn't create the translation");
                }

                return new APIResponse(201, true, "Translation created successfully", result);

            } catch (Exception ex)
            {
                return new APIResponse(500, false, ex.Message);
            }

        }

        [HttpDelete("delete-transaction-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> deleteTransaction(Guid id)
        {
            if (id == null)
            {
                return new APIResponse(404, false, "Please provide a valid transaction ID");
            }

            TranslationModel translationModel = await _translationRepository.deleteTranslationById(id);

            if (translationModel == null)
            {
                return new APIResponse(406, false, "Something went wrong while deleting");
            }

            return new APIResponse(201, true, "Successfully deleted", translationModel);
        }

        [HttpPatch("update-translation")]
        public async Task<ActionResult<APIResponse>> updateSession([FromBody] TranslationModel translation)
        {
            try
            {
                if (translation.translationID == Guid.Empty)
                { 
                    return (new APIResponse(406, false, "Please provide a valid translation ID"));
                }

                if (await _sessionsRepository.getSessionById(translation.sessionID) == null)
                {
                    return (new APIResponse(204, false, "Session is not found"));
                }

                TranslationModel updatedTranslation = await _translationRepository.updateTranslation(translation);

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

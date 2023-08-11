using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;


namespace Be_My_Voice_Backend.Controllers
{
    [Route("api/translation")]
    public class TranslationController : Controller
    {
        private readonly ITranslationsRepository _translationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly HttpClient _httpClient;

        public TranslationController(ITranslationsRepository translationRepository, IUserRepository userRepository, ISessionsRepository sessionsRepository)
        {
            this._translationRepository = translationRepository;
            this._userRepository = userRepository;
            this._sessionsRepository = sessionsRepository;
            this._httpClient = new HttpClient();
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

                //if (session.endDate.CompareTo(DateTime.Now) < 0)
                //{
                //    await _sessionsRepository.updateSessionStatus(
                //        new UpdateSessionStatusDTO
                //        {
                //            sessionID = session.sessionID,
                //            status = "completed"
                //        }
                //    );

                //    return new APIResponse(406, false, "Session has expired");
                //}

                TranslationModel translation = new TranslationModel();
                translation.sessionID = session.sessionID;
                translation.translatedText = predictedText;

                if (createTranslationDTO.userType == "mute")
                {
                    translation.userType = "mute";

                    // TODO: API call to infer ML model

                } else
                {
                    translation.userType = "normal";
                    translation.translatedText = createTranslationDTO.resultObjectFromSkeleton;
                }

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

            }
            catch (Exception ex)
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

        [HttpPost("translate-audio-to-text")]
        // get a file from the request body
        public async Task<ActionResult<APIResponse>> TranslateAudioToText([FromBody] string base64Audio)
        {
            try
            {
                if (base64Audio == null)
                {
                    return new APIResponse(406, false, "Please provide a valid audio file in base64 format", base64Audio);
                }

                var requestBody = new
                {
                    config = new
                    {
                        enableAutomaticPunctuation = true,
                        encoding = "WEBM_OPUS",
                        languageCode = "si-LK",
                        model = "default"
                    },
                    audio = new
                    {
                        content = base64Audio
                    }
                };

                var requestJson = JsonConvert.SerializeObject(requestBody);

                using (var client = new HttpClient())
                {
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(
                        "https://speech.googleapis.com/v1p1beta1/speech:recognize?key=AIzaSyAc8sXVyk520hND8a0mEDy_l149FkDTB5A",
                        content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        // Parse the JSON response to extract the translated text
                        // Assuming you have a method to parse JSON and get the translated text
                        string translatedText = ParseResponseAndGetTranslatedText(responseContent);

                        return new APIResponse(200, true, "Successfully translated", translatedText);
                    }
                    else
                    {
                        return new APIResponse(500, false, "Error translating audio");
                    }
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(500, false, ex.Message);
            }
        }

        [HttpPost("translate-text-to-sinhala")]
        public async Task<ActionResult<APIResponse>> TranslateTextToAudio(string text)
        {
            try
            {
                if (text == null)
                {
                    return new APIResponse(406, false, "Please provide a valid text");
                }

                string requestURLToSubasa = "https://marytts.subasa.lk/process?INPUT_TYPE=TEXT&AUDIO=WAVE_FILE&OUTPUT_TYPE=AUDIO&LOCALE=si&INPUT_TEXT=" + "\"" + text + "\"";

                HttpResponseMessage response = await _httpClient.GetAsync(requestURLToSubasa);

                if (response.IsSuccessStatusCode)
                {
                    byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
                    string base64Audio = Convert.ToBase64String(audioBytes);

                    return new APIResponse(200, true, "Audio generated successfully", base64Audio);
                }
                else
                {
                    return new APIResponse(500, false, "Error generating audio");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(500, false, ex.Message);
            }
        }

        private string ParseResponseAndGetTranslatedText(string responseContent)
        {
            try
            {
                JObject jsonResponse = JObject.Parse(responseContent);

                // The structure of the response might vary, but typically you can find the transcript here
                var transcripts = jsonResponse["results"]?.SelectTokens("..transcript").Select(t => (string)t).ToList();

                if (transcripts != null && transcripts.Count > 0)
                {
                    return string.Join(" ", transcripts);
                }
                else
                {
                    return "No translated text available.";
                }
            }
            catch (Exception ex)
            {
                // Handle any parsing errors gracefully
                return $"Error parsing response: {ex.Message}";
            }


        }

    }
}

using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Be_My_Voice_Backend.Controllers
{
    [Route("api/quiz")]
    public class QuizController : Controller
    {
        private readonly ITranslationsRepository _translationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly HttpClient _httpClient;
        private readonly IQuizRepository _quizRepository;

        public QuizController(ITranslationsRepository translationRepository, IUserRepository userRepository, ISessionsRepository sessionsRepository, IQuizRepository quizRepository)
            {
            this._translationRepository = translationRepository;
            this._userRepository = userRepository;
            this._sessionsRepository = sessionsRepository;
            this._httpClient = new HttpClient();
            this._quizRepository = quizRepository;
        }

        [HttpGet("/get-all-quizzes")]
        public async Task<ActionResult<APIResponse>> GetAllQuizzes()
        {
            try
            {
                QuizModel[] quizzes = await _quizRepository.getAllQuizzes();
                return (new APIResponse(200, true, "All quizzes in the database", quizzes));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message));
            }

        }

        [HttpGet("/get-quiz/{:id}")]
        public async Task<ActionResult<APIResponse>> GetQuiz(Guid quizID)
        {
            try
            {
                QuizModel quiz = await _quizRepository.getQuizById(quizID);
                return (new APIResponse(200, true, "Quiz Found", quiz));
            }
            catch (Exception ex)
            {
                return (new APIResponse(500, false, ex.Message, ex.StackTrace));
            }
        }

        [HttpPost("create-quiz")]
        public async Task<ActionResult<APIResponse>> createQuiz([FromBody] CreateQuizDTO createQuizDTO)
        {
            try
            { 

                QuizModel quiz = new QuizModel();
                quiz.QuizName = createQuizDTO.QuizName;
                quiz.QuizDescription = createQuizDTO.QuizDescription;
                quiz.QuizVideo = createQuizDTO.QuizVideo;
                quiz.QuizAnswers = createQuizDTO.QuizAnswers;
                quiz.CorrectAnswer = createQuizDTO.CorrectAnswer;

                QuizModel result = await _quizRepository.createQuiz(quiz);

                if (result == null)
                {
                    return new APIResponse(500, false, "Couldn't create the quiz");
                }

                return new APIResponse(201, true, "Quiz created successfully", result);

            }
            catch (Exception ex)
            {
                return new APIResponse(500, false, ex.Message);
            }

        }

        [HttpDelete("delete-quiz-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> deleteQuiz(Guid id)
        {
            if (id == null)
            {
                return new APIResponse(404, false, "Please provide a valid quiz ID");
            }

            QuizModel quizModel = await _quizRepository.deleteQuizById(id);

            if (quizModel == null)
            {
                return new APIResponse(406, false, "Something went wrong while deleting");
            }

            return new APIResponse(201, true, "Successfully deleted", quizModel);
        }

        [HttpPatch("update-quiz")]
        public async Task<ActionResult<APIResponse>> updateQuiz([FromBody] QuizModel quiz)
        {
            try
            {
                QuizModel updatedQuiz = await _quizRepository.updateQuiz(quiz);

                if (updatedQuiz != null)
                {
                    return (new APIResponse(200, true, "Quiz updated", updatedQuiz));
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

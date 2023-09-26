using Be_My_Voice_Backend.Models;

namespace Be_My_Voice_Backend.Repository.IRepository
{
    public interface IQuizRepository
    {
        Task<QuizModel[]> getAllQuizzes();
        Task<QuizModel> getQuizById(Guid id);
        Task<QuizModel> createQuiz(QuizModel Quiz);
        Task<QuizModel> updateQuiz(QuizModel Quiz);
        Task<QuizModel> deleteQuizById(Guid id);
    }
}

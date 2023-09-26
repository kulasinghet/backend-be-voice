using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Be_My_Voice_Backend.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public QuizRepository(ApplicationDBContext dbCotetxt)
        {
            _dbContext = dbCotetxt;
        }

        public async Task<QuizModel> createQuiz(QuizModel Quiz)
        {
            await _dbContext.quizModels.AddAsync(Quiz);
            await _dbContext.SaveChangesAsync();
            return Quiz;
        }

        public async Task<QuizModel> deleteQuizById(Guid id)
        {
            QuizModel temp = await _dbContext.quizModels.FirstOrDefaultAsync(t => t.QuizID == id);

            _dbContext.quizModels.Remove(temp);
            await _dbContext.SaveChangesAsync();
            return temp;
        }

        public async Task<QuizModel[]> getAllQuizzes()
        {
            return await _dbContext.quizModels.ToArrayAsync();
        }

        public async Task<QuizModel> getQuizById(Guid id)
        {
            return await _dbContext.quizModels.FirstOrDefaultAsync(t => t.QuizID == id);
        }

        public async Task<QuizModel> updateQuiz(QuizModel Quiz)
        {
            _dbContext.quizModels.Update(Quiz);
            await _dbContext.SaveChangesAsync();
            return Quiz;
        }
    }
}

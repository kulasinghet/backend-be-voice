using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Be_My_Voice_Backend.Repository
{
    public class TranslationsRepository : ITranslationsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public TranslationsRepository(ApplicationDBContext dbCotetxt)
        {
            _dbContext = dbCotetxt;
        }
        
        public async Task<TranslationModel> createTranslation(TranslationModel translation)
        {
            await _dbContext.translations.AddAsync(translation);
            await _dbContext.SaveChangesAsync();
            return translation;
        }

        public async Task<TranslationModel> deleteTranslationById(Guid id)
        {
            TranslationModel temp = await _dbContext.translations.FirstOrDefaultAsync(t => t.translationID == id);

            _dbContext.translations.Remove(temp);
            await _dbContext.SaveChangesAsync();
            return temp;
        }

        public async Task<TranslationModel> deleteTranslationn(TranslationModel translation)
        {
            _dbContext.translations.Remove(translation);
            await _dbContext.SaveChangesAsync();
            return translation;
        }

        public async Task<TranslationModel[]> getAllTranslations()
        {
            return await _dbContext.translations.Include(t => t.session).ToArrayAsync();
        }

        public async Task<TranslationModel[]> getlTranslationBySessionID(Guid id)
        {
            return await _dbContext.translations.Where(s => s.sessionID == id).Include(t => t.session).OrderByDescending(s => s.createdTime).ToArrayAsync();
        }

        public async Task<TranslationModel> getTranslationById(Guid id)
        {
            return await _dbContext.translations.Include(t => t.session).FirstOrDefaultAsync(t => t.translationID == id);
        }

        public async Task<TranslationModel> updateTranslation(TranslationModel translation)
        {
            _dbContext.translations.Update(translation);
            await _dbContext.SaveChangesAsync();
            return translation;
        }
    }
}

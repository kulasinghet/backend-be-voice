using Be_My_Voice_Backend.Models;

namespace Be_My_Voice_Backend.Repository.IRepository
{
    public interface ITranslationsRepository
    {
        Task<TranslationModel[]> getAllTranslations();
        Task<TranslationModel> getTranslationById(Guid id);
        Task<TranslationModel[]> getlTranslationBySessionID(Guid id);
        Task<TranslationModel> createTranslation(TranslationModel translation);
        Task<TranslationModel> updateTranslation(TranslationModel translation);
        Task<TranslationModel> deleteTranslationn(TranslationModel translation);
        Task<TranslationModel> deleteTranslationById(Guid id);

    }
}

using Be_My_Voice_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Be_My_Voice_Backend.Repository.IRepository
{
    public interface INormalUserTranslationsRepository
    {
        Task<NormalUserTranslationModel[]> NormalUserTranslationModel();
        Task<NormalUserTranslationModel> getNormalUserTranslationById(Guid id);
        Task<NormalUserTranslationModel[]> getNormalUserTranslationBySessionID(Guid id);
        Task<NormalUserTranslationModel> createNormalUserTranslation(NormalUserTranslationModel translation, [FromForm] IFormFile voiceRecord);
        Task<NormalUserTranslationModel> updateNormalUserTranslation(NormalUserTranslationModel translation);
        Task<NormalUserTranslationModel> deleteNormalUserTranslation(NormalUserTranslationModel translation);
        Task<NormalUserTranslationModel> deleteNormalUserTranslationById(Guid id);

    }
}

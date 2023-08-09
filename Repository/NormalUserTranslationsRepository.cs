using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Be_My_Voice_Backend.Repository
{
    public class NormalUserTranslationsRepository : INormalUserTranslationsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public NormalUserTranslationsRepository(ApplicationDBContext dbCotetxt)
        {
            _dbContext = dbCotetxt;
        }

        public async Task<NormalUserTranslationModel> createNormalUserTranslation(NormalUserTranslationModel translation, [FromForm] IFormFile voiceRecord)
        {
            // TODO: Implement this method

            // if content type is not mp3 or wav return bad request
            if (voiceRecord.ContentType != "audio/wav" && voiceRecord.ContentType != "audio/wave" && voiceRecord.ContentType != "audio/x-wav" && voiceRecord.ContentType != "audio/mp3")
            {
                return null;
            }
            var text = await CovnvertSpeechToTextApiCall(ConvertToByteArrayContent(voiceRecord));


            await _dbContext.NormalUsertranslations.AddAsync(translation);
            await _dbContext.SaveChangesAsync();
            return translation;
        }

        private static Task<String> CovnvertSpeechToTextApiCall(ByteArrayContent byteArrayContent)
        {
            Console.WriteLine("Converting speech to text");
            return null;

        }

        private ByteArrayContent ConvertToByteArrayContent(IFormFile audofile)
        {
            byte[] data;

            using (var br = new BinaryReader(audofile.OpenReadStream()))
            {
                data = br.ReadBytes((int)audofile.OpenReadStream().Length);
            }

            return new ByteArrayContent(data);
        }

        public async Task<NormalUserTranslationModel> deleteNormalUserTranslationById(Guid id)
        {
            NormalUserTranslationModel temp = await _dbContext.NormalUsertranslations.FirstOrDefaultAsync(t => t.NormalUserTranslationID == id);

            _dbContext.NormalUsertranslations.Remove(temp);
            await _dbContext.SaveChangesAsync();
            return temp;
        }

        public async Task<NormalUserTranslationModel> deleteNormalUserTranslation(NormalUserTranslationModel translation)
        {
            _dbContext.NormalUsertranslations.Remove(translation);
            await _dbContext.SaveChangesAsync();
            return translation;
        }

        public async Task<NormalUserTranslationModel[]> NormalUserTranslationModel()
        {
            return await _dbContext.NormalUsertranslations.Include(t => t.Session).ToArrayAsync();
        }

        public async Task<NormalUserTranslationModel[]> getNormalUserTranslationBySessionID(Guid id)
        {
            return await _dbContext.NormalUsertranslations.Where(s => s.SessionID == id).Include(t => t.Session).OrderByDescending(s => s.CreatedTime).ToArrayAsync();
        }

        public async Task<NormalUserTranslationModel> getNormalUserTranslationById(Guid id)
        {
            return await _dbContext.NormalUsertranslations.Include(t => t.Session).FirstOrDefaultAsync(t => t.NormalUserTranslationID == id);
        }

        public async Task<NormalUserTranslationModel> updateNormalUserTranslation(NormalUserTranslationModel translation)
        {
            _dbContext.NormalUsertranslations.Update(translation);
            await _dbContext.SaveChangesAsync();
            return translation;
        }

    }
}

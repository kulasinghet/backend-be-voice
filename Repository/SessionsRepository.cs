using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Be_My_Voice_Backend.Repository
{
    public class SessionsRepository : ISessionsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public SessionsRepository(ApplicationDBContext dbCotetxt)
        {
            _dbContext = dbCotetxt;
        }

        public async Task<SessionModel[]> getAllSessions()
        {
            return await _dbContext.sessions.Include(u => u.user).ToArrayAsync();
        }

        public async Task<SessionModel> getSessionById(Guid id)
        {
            return await _dbContext.sessions.Include(u => u.user).FirstOrDefaultAsync(s => s.sessionID == id);
        }

        public Task<SessionModel[]> getSessionsByUserId(Guid id)
        {
            return _dbContext.sessions.Where(s => s.userID == id).Include(u => u.user).ToArrayAsync();
        }

        public async Task<SessionModel> createSession(SessionModel session)
        {
            await _dbContext.sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }

        public async Task<SessionModel> updateSession(SessionModel session)
        {
            _dbContext.sessions.Update(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }

        public async Task<SessionModel> deleteSession(SessionModel session)
        {
            _dbContext.sessions.Remove(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }

        public async Task<SessionModel> deleteSessionById(Guid id)
        {
            SessionModel session = await getSessionById(id);
            _dbContext.sessions.Remove(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }



    }
}

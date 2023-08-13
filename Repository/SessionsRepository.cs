using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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

        public async Task<SessionModel> updateSessionStatus(UpdateSessionStatusDTO sessionStatus)
        {
            SessionModel session = await _dbContext.sessions.FirstOrDefaultAsync(s => s.sessionID == sessionStatus.sessionID);
            UserModel userModel = await _dbContext.users.FirstOrDefaultAsync(u => u.UserID == session.userID);

            var tempSession = new SessionModel()
            {
                sessionID = session.sessionID,
                userID = session.userID,
                user = userModel,
                startDate = session.startDate,
                endDate = session.endDate,
                status = sessionStatus.status
            };


            _dbContext.sessions.Remove(session);
            await _dbContext.SaveChangesAsync();

            _dbContext.sessions.AddAsync(tempSession);
            await _dbContext.SaveChangesAsync();

            return tempSession;
        }
    }
}

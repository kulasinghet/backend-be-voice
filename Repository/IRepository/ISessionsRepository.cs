using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Models.DTO;

namespace Be_My_Voice_Backend.Repository.IRepository
{
    public interface ISessionsRepository
    {
        Task<SessionModel[]> getAllSessions();
        Task<SessionModel> getSessionById(Guid id);
        Task<SessionModel[]> getSessionsByUserId(Guid id);
        Task<SessionModel> createSession(SessionModel session);
        Task<SessionModel> updateSession(SessionModel session);
        Task<SessionModel> deleteSession(SessionModel session);
        Task<SessionModel> deleteSessionById(Guid id);
        Task<SessionModel> updateSessionStatus(UpdateSessionStatusDTO sessionStatus);
    }
}

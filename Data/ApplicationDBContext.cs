
using Be_My_Voice_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Be_My_Voice_Backend.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<UserModel> users { get; set; }
        public DbSet<ChatModel> chats { get; set; }
        public DbSet<SessionModel> sessions { get; set; }
        public DbSet<TranslationModel> translations { get; set; }
        public DbSet<NormalUserTranslationModel> NormalUsertranslations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

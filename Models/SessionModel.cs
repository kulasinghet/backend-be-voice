using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Be_My_Voice_Backend.Models
{
    public class SessionModel
    {
        [Key]
        public Guid sessionID { get; set; } = Guid.Empty;

        public Guid userID { get; set; }
        
        [ForeignKey("userID")]
        public UserModel user { get; set; }
        
        public DateTime startDate { get; set; }
        
        public DateTime endDate { get; set; }
        public string status { get; set; } = "waiting";
    }
}

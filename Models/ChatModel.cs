using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Be_My_Voice_Backend.Models
{
    public class ChatModel
    {
        [Key]
        public Guid chatID { get; set; }
        public string message { get; set; }
        public DateTime updatedTime { get; set; }
        public string videoUrl { get; set; }
        public string status { get; set; }
        public Guid sentUserID { get; set; }
        public Guid receivedUserID { get; set; }
        [ForeignKey("sentUserID")]
        public UserModel sentUser { get; set; }
        [ForeignKey("receivedUserID")]
        public UserModel receivedUser { get; set; }
    }
}

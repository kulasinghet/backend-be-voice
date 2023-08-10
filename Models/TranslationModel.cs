using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Be_My_Voice_Backend.Models
{
    public class TranslationModel
    {
        [Key]
        public Guid translationID { get; set; } = Guid.NewGuid();
        public Guid sessionID { get; set; }
        [ForeignKey("sessionID")]
        public SessionModel session { get; set; }
        public String translatedText { get; set; }
        public string videoUrl { get; set; } = "";
        public DateTime createdTime { get; set; } = DateTime.Now;
        public int status { get; set; } = 0;
        public string userType { get; set; } = "";
    }
}

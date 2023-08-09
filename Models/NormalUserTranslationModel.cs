using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Be_My_Voice_Backend.Models
{
    public class NormalUserTranslationModel
    {
        [Key]
        public Guid NormalUserTranslationID { get; set; } = Guid.NewGuid();
        public Guid SessionID { get; set; }
        [ForeignKey("SessionID")]
        public SessionModel Session { get; set; }
        public String NormalUserTranslatedText { get; set; }
        public string VideoUrl { get; set; } = "";
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public int Status { get; set; } = 0;
    }
}

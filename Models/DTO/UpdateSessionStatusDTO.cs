using System.ComponentModel.DataAnnotations.Schema;

namespace Be_My_Voice_Backend.Models.DTO
{
    public class UpdateSessionStatusDTO
    {
        public Guid sessionID { get; set; } = Guid.Empty;
        public string status { get; set; } = "waiting";
    }
}

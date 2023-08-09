namespace Be_My_Voice_Backend.Models.DTO
{
    public class CreateTranslationDTO
    {
        public Guid sessionID { get; set; }
        public Guid userID { get; set; }
        public string resultObjectFromSkeleton { get; set; }
    }
}

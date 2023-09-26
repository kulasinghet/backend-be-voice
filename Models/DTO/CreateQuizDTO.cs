namespace Be_My_Voice_Backend.Models.DTO
{
    public class CreateQuizDTO
    {
        public String QuizName { get; set; }
        public String QuizDescription { get; set; } = string.Empty;
        public String QuizType { get; set; } = string.Empty;
        public String QuizVideo { get; set; } = string.Empty;
        public String QuizAnswers { get; set; } 
        public String CorrectAnswer { get; set; } = string.Empty;
    }
}

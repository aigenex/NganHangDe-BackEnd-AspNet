namespace NganHangDe_Backend.Models
{
    public class QuestionGroupInfo : QuestionGroup
    {
        public List<Question> Questions { get; set; } = null!;
        public Subject Subject { get; set; } = null!;

    }
}

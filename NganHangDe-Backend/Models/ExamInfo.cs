namespace NganHangDe_Backend.Models
{
    public class ExamInfo : Exam
    {
        public Subject? Subject { get; set; }
        public Question[]? Questions { get; set; }
    }
}

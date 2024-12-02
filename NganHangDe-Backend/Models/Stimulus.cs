namespace NganHangDe_Backend.Models
{
    public class Stimulus
    {
        public string Title { get; set; } = null!;
        public string? Instruction { get; set; } = "";
        public string Content { get; set; } = null!;

        public Question[] Questions { get; set; } = null!;

        public Stimulus(string title, string instruction, string content, Question[] questions)
        {
            Title = title;
            Instruction = instruction;
            Content = content;
            Questions = questions;
        }

        public int GetQuestionCount()
        {
            return Questions.Length;
        }

        public void AddQuestion(Question question)
        {
            Questions.Append(question);
        }

        public void RemoveQuestion(Question question)
        {
            Questions = Questions.Where(q => q != question).ToArray();
        }

        public void RemoveQuestion(int index)
        {
            Questions = Questions.Where((q, i) => i != index).ToArray();
        }





    }
}

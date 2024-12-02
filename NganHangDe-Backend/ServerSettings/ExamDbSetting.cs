namespace NganHangDe_Backend.ServerSettings
{
    public class ExamDbSetting
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string QuestionCollectionName { get; set; } = null!;
        public string SubjectCollectionName { get; set; } = null!;
        public string QuestionGroupCollectionName { get; set; } = null!;
        public string ExamCollectionName { get; set; } = null!;
        public string QuestionBankCollectionName { get; set; } = null!;
    }
}

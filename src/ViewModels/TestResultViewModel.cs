namespace WarehouseManagementSchool.ViewModels;

public sealed class TestResultViewModel
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MaximumScore { get; set; }
    public int Grade { get; set; }
    public double Percentage { get; set; }
    public int AnsweredQuestions { get; set; }
    public DateTime DateTaken { get; set; }
    public string FeedbackMessage { get; set; } = string.Empty;
    public string FeedbackClass { get; set; } = "secondary";
}

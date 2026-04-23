namespace WarehouseManagementSchool.ViewModels;

public sealed class TestStartViewModel
{
    public IReadOnlyList<TestQuestionViewModel> Questions { get; set; } = [];
    public int TotalPoints { get; set; }
    public string? ErrorMessage { get; set; }
}

public sealed class TestQuestionViewModel
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int Points { get; set; }
    public IReadOnlyList<TestAnswerViewModel> Answers { get; set; } = [];
}

public sealed class TestAnswerViewModel
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

namespace WarehouseManagementSchool.DTOs;

public sealed class GeneratedTestDto
{
    public IReadOnlyList<GeneratedQuestionDto> Questions { get; set; } = [];
    public int TotalPoints { get; set; }
}

public sealed class GeneratedQuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int Points { get; set; }
    public IReadOnlyList<GeneratedAnswerDto> Answers { get; set; } = [];
}

public sealed class GeneratedAnswerDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

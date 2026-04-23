namespace WarehouseManagementSchool.DTOs;

public sealed class TestSubmissionDto
{
    public IReadOnlyList<SubmittedAnswerDto> Answers { get; set; } = [];
}

public sealed class SubmittedAnswerDto
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
}

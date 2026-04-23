using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSchool.Models;

public sealed class TestResultItem
{
    public int Id { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int TestResultId { get; set; }

    [ForeignKey(nameof(TestResultId))]
    public TestResult? TestResultNavigation { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question? QuestionNavigation { get; set; }

    public bool IsCorrect { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSchool.Models;

public sealed class Answer
{
    public int Id { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question? QuestionNavigation { get; set; }
}

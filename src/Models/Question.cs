using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSchool.Models;

public sealed class Question
{
    public int Id { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Text { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? CategoryNavigation { get; set; }

    [Required]
    public DifficultyLevel Difficulty { get; set; }

    [Range(1, 3)]
    public int Points { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}

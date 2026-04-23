using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSchool.Models;

public sealed class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

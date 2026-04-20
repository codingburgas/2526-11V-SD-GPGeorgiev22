using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSchool.DTOs;

public sealed class CreateCategoryDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}

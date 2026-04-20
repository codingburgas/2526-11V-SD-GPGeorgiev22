using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSchool.ViewModels;

public sealed class LoginViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}

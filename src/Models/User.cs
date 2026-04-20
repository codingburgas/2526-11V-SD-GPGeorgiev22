using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSchool.Models;

public sealed class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(512)]
    public string PasswordHash { get; set; } = string.Empty;
}

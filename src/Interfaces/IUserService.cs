using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Interfaces;

public interface IUserService
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> UsernameExistsAsync(string username);
    Task<User> RegisterAsync(RegisterUserDto dto);
    Task<bool> ValidateCredentialsAsync(LoginUserDto dto);
}

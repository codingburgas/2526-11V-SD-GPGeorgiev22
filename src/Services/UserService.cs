using Microsoft.EntityFrameworkCore;
using WarehouseManagementSchool.Data;
using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Helpers;
using WarehouseManagementSchool.Interfaces;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Services;

public sealed class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasherService _passwordHasherService;

    public UserService(
        ApplicationDbContext context,
        IPasswordHasherService passwordHasherService)
    {
        _context = context;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var normalizedUsername = TextSanitizer.TrimSafe(username);

        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == normalizedUsername);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var normalizedUsername = TextSanitizer.TrimSafe(username);

        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == normalizedUsername);
    }

    public async Task<User> RegisterAsync(RegisterUserDto dto)
    {
        var normalizedUsername = TextSanitizer.TrimSafe(dto.Username);
        var normalizedPassword = TextSanitizer.TrimSafe(dto.Password);

        if (await UsernameExistsAsync(normalizedUsername))
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var user = new User
        {
            Username = normalizedUsername,
            PasswordHash = _passwordHasherService.HashPassword(normalizedPassword)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> ValidateCredentialsAsync(LoginUserDto dto)
    {
        var normalizedUsername = TextSanitizer.TrimSafe(dto.Username);
        var normalizedPassword = TextSanitizer.TrimSafe(dto.Password);

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == normalizedUsername);

        if (user is null)
        {
            return false;
        }

        return _passwordHasherService.VerifyPassword(normalizedPassword, user.PasswordHash);
    }
}

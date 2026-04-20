using Microsoft.EntityFrameworkCore;
using WarehouseManagementSchool.Data;
using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Helpers;
using WarehouseManagementSchool.Interfaces;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = TextSanitizer.TrimSafe(dto.Name)
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
}

using Microsoft.EntityFrameworkCore;
using WarehouseManagementSchool.Data;
using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Helpers;
using WarehouseManagementSchool.Interfaces;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Services;

public sealed class LessonService : ILessonService
{
    private readonly ApplicationDbContext _context;

    public LessonService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Lesson>> GetAllAsync()
    {
        return await _context.Lessons
            .AsNoTracking()
            .Include(l => l.CategoryNavigation)
            .OrderBy(l => l.Title)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _context.Lessons
            .AsNoTracking()
            .Include(l => l.CategoryNavigation)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IReadOnlyList<Lesson>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Lessons
            .AsNoTracking()
            .Include(l => l.CategoryNavigation)
            .Where(l => l.CategoryId == categoryId)
            .OrderBy(l => l.Title)
            .ToListAsync();
    }

    public async Task<Lesson> CreateAsync(CreateLessonDto dto)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

        if (category is null)
        {
            throw new InvalidOperationException("Category does not exist.");
        }

        // Duplicate category name is stored intentionally to keep lesson snapshots readable in reports.
        var lesson = new Lesson
        {
            Title = TextSanitizer.TrimSafe(dto.Title),
            Content = TextSanitizer.TrimSafe(dto.Content),
            CategoryId = dto.CategoryId,
            Category = category.Name
        };

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }
}

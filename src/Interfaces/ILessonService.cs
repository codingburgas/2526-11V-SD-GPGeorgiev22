using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Interfaces;

public interface ILessonService
{
    Task<IReadOnlyList<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(int id);
    Task<IReadOnlyList<Lesson>> GetByCategoryIdAsync(int categoryId);
    Task<Lesson> CreateAsync(CreateLessonDto dto);
}

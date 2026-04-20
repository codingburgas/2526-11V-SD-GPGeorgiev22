using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(CreateCategoryDto dto);
}

using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSchool.Interfaces;

namespace WarehouseManagementSchool.Controllers;

public sealed class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ILessonService _lessonService;

    public CategoriesController(
        ICategoryService categoryService,
        ILessonService lessonService)
    {
        _categoryService = categoryService;
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        return View(categories);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var category = await _categoryService.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        ViewData["CategoryName"] = category.Name;
        ViewData["CategoryId"] = category.Id;

        var lessons = await _lessonService.GetByCategoryIdAsync(id);
        return View(lessons);
    }
}

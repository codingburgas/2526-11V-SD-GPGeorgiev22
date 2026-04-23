using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSchool.Interfaces;
using WarehouseManagementSchool.ViewModels;

namespace WarehouseManagementSchool.Controllers;

public sealed class LessonsController : Controller
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var lessons = await _lessonService.GetAllAsync();

        var model = lessons
            .Select(lesson => new LessonListItemViewModel
            {
                Id = lesson.Id,
                Title = lesson.Title,
                CategoryName = lesson.CategoryNavigation?.Name ?? lesson.Category,
                Summary = lesson.Content.Length > 140
                    ? $"{lesson.Content[..140]}..."
                    : lesson.Content
            })
            .ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson is null)
        {
            return NotFound();
        }

        var model = new LessonDetailsViewModel
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            CategoryName = lesson.CategoryNavigation?.Name ?? lesson.Category
        };

        return View(model);
    }
}

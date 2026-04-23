using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSchool.Interfaces;
using WarehouseManagementSchool.ViewModels;

namespace WarehouseManagementSchool.Controllers;

public sealed class StatisticsController : Controller
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var overview = await _statisticsService.GetOverview();

        var model = new StatisticsViewModel
        {
            TotalTestsTaken = overview.TotalTestsTaken,
            HighestScore = overview.HighestScore,
            LowestScore = overview.LowestScore,
            AverageScore = overview.AverageScore,
            CategorySuccessRates = overview.CategorySuccessRates
                .Select(rate => new StatisticsCategoryRateViewModel
                {
                    CategoryName = rate.CategoryName,
                    SuccessRate = rate.SuccessRate,
                    CorrectAnswers = rate.CorrectAnswers,
                    TotalAnswers = rate.TotalAnswers
                })
                .ToList()
        };

        return View(model);
    }
}

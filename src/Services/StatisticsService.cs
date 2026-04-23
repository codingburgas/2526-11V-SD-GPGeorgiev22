using Microsoft.EntityFrameworkCore;
using WarehouseManagementSchool.Data;
using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Interfaces;

namespace WarehouseManagementSchool.Services;

public sealed class StatisticsService : IStatisticsService
{
    private static readonly int[] TestCategoryIds = [4, 5, 6];
    private readonly ApplicationDbContext _context;

    public StatisticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsOverviewDto> GetOverview()
    {
        var allResults = await _context.TestResults
            .AsNoTracking()
            .ToListAsync();

        var totalTests = allResults.Count;

        var overview = new StatisticsOverviewDto
        {
            TotalTestsTaken = totalTests,
            HighestScore = totalTests == 0 ? 0 : allResults.Max(result => result.Score),
            LowestScore = totalTests == 0 ? 0 : allResults.Min(result => result.Score),
            AverageScore = totalTests == 0 ? 0 : Math.Round(allResults.Average(result => result.Score), 2)
        };

        var rawRates = await _context.TestResultItems
            .AsNoTracking()
            .Include(item => item.QuestionNavigation)
            .ThenInclude(question => question!.CategoryNavigation)
            .Where(item => item.QuestionNavigation != null && TestCategoryIds.Contains(item.QuestionNavigation.CategoryId))
            .GroupBy(item => new
            {
                item.QuestionNavigation!.CategoryId,
                CategoryName = item.QuestionNavigation.CategoryNavigation != null
                    ? item.QuestionNavigation.CategoryNavigation.Name
                    : "Uncategorized"
            })
            .Select(group => new CategorySuccessRateDto
            {
                CategoryName = group.Key.CategoryName,
                CorrectAnswers = group.Count(item => item.IsCorrect),
                TotalAnswers = group.Count(),
                SuccessRate = group.Count() == 0
                    ? 0
                    : Math.Round((double)group.Count(item => item.IsCorrect) / group.Count() * 100, 2)
            })
            .ToListAsync();

        var categoryNames = await _context.Categories
            .AsNoTracking()
            .Where(category => TestCategoryIds.Contains(category.Id))
            .OrderBy(category => category.Id)
            .Select(category => category.Name)
            .ToListAsync();

        var completedRates = new List<CategorySuccessRateDto>();

        foreach (var categoryName in categoryNames)
        {
            var existing = rawRates.FirstOrDefault(rate => rate.CategoryName == categoryName);
            completedRates.Add(existing ?? new CategorySuccessRateDto
            {
                CategoryName = categoryName,
                SuccessRate = 0,
                CorrectAnswers = 0,
                TotalAnswers = 0
            });
        }

        overview.CategorySuccessRates = completedRates;
        return overview;
    }
}

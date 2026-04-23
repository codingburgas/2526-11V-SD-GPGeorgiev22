namespace WarehouseManagementSchool.ViewModels;

public sealed class StatisticsViewModel
{
    public int TotalTestsTaken { get; set; }
    public double HighestScore { get; set; }
    public double LowestScore { get; set; }
    public double AverageScore { get; set; }
    public IReadOnlyList<StatisticsCategoryRateViewModel> CategorySuccessRates { get; set; } = [];
}

public sealed class StatisticsCategoryRateViewModel
{
    public string CategoryName { get; set; } = string.Empty;
    public double SuccessRate { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalAnswers { get; set; }
}

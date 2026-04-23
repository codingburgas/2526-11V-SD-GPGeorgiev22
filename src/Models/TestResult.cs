using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSchool.Models;

public sealed class TestResult
{
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int Score { get; set; }

    [Range(0, int.MaxValue)]
    public int MaximumScore { get; set; }

    [Range(2, 6)]
    public int Grade { get; set; }

    public DateTime DateTaken { get; set; }

    public ICollection<TestResultItem> TestResultItems { get; set; } = new List<TestResultItem>();
}

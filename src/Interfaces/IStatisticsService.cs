using WarehouseManagementSchool.DTOs;

namespace WarehouseManagementSchool.Interfaces;

public interface IStatisticsService
{
    Task<StatisticsOverviewDto> GetOverview();
}

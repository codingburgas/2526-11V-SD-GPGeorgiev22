using WarehouseManagementSchool.Interfaces;

namespace WarehouseManagementSchool.Services;

public sealed class GradingService : IGradingService
{
    public int CalculateGrade(double percentage)
    {
        if (percentage >= 90)
        {
            return 6;
        }

        if (percentage >= 75)
        {
            return 5;
        }

        if (percentage >= 60)
        {
            return 4;
        }

        if (percentage >= 45)
        {
            return 3;
        }

        return 2;
    }
}

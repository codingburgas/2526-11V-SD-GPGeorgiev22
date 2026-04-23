using WarehouseManagementSchool.DTOs;

namespace WarehouseManagementSchool.Interfaces;

public interface ITestService
{
    Task<GeneratedTestDto> GenerateTest();
    Task<TestEvaluationDto> EvaluateTest(TestSubmissionDto submission);
    Task<TestEvaluationDto?> GetResult(int id);
}

namespace WarehouseManagementSchool.ViewModels;

public sealed class LessonDetailsViewModel
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
}

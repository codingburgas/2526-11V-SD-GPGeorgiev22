namespace WarehouseManagementSchool.Helpers;

public static class TextSanitizer
{
    public static string TrimSafe(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim();
    }
}

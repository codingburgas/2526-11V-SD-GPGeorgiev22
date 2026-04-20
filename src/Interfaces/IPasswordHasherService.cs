namespace WarehouseManagementSchool.Interfaces;

public interface IPasswordHasherService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

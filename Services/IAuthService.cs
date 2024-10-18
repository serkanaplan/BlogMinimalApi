using BlogMinimalApi.Models;

namespace BlogMinimalApi.Services;

public interface IAuthService
{
    Task<string> GenerateJwtToken(ApplicationUser user);
    Task<(bool Success, string Message)> Register(Register model);
    Task<(bool Success, string Message, string Token)> Login(Login model);
}

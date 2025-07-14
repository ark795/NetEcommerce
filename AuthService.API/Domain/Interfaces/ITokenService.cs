using AuthService.API.Domain.Entities;
namespace AuthService.API.Domain.Interfaces;
public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
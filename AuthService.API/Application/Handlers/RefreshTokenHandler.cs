using AuthService.API.Application.Commands;
using AuthService.API.Application.DTOs;
using AuthService.API.Domain.Interfaces;
using AuthService.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace AuthService.API.Application.Handlers;
public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, UserDto>
{
    private readonly AuthDbContext _db;
    private readonly ITokenService _tokenService;
    public RefreshTokenHandler(AuthDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }
    public async Task<UserDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new Exception("Invalid refresh token.");
        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _db.SaveChangesAsync(cancellationToken);
        return new UserDto
        {
            Email = user.Email,
            Username = user.Username,
            Role = user.Role,
            AccessToken = _tokenService.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken
        };
    }
}
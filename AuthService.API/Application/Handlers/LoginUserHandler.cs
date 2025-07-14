using AuthService.API.Application.Commands;
using AuthService.API.Application.DTOs;
using AuthService.API.Contracts.Events;
using AuthService.API.Domain.Entities;
using AuthService.API.Domain.Interfaces;
using AuthService.API.Infrastructure.Data;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace AuthService.API.Application.Handlers;
public class LoginUserHandler : IRequestHandler<LoginUserCommand, UserDto>
{
    private readonly AuthDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IPublishEndpoint _publishEndpoint;
    public LoginUserHandler(AuthDbContext db, ITokenService tokenService, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _tokenService = tokenService;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user is null)
            throw new Exception("Invalid credentials.");
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Invalid credentials.");
        // Refresh token جدید 
        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _db.SaveChangesAsync(cancellationToken);
        var @event = new UserLoggedInEvent
        {
            UserId = user.Id,
            Email = user.Email,
        };
        await _publishEndpoint.Publish(@event, cancellationToken);
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
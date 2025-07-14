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
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly AuthDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IPublishEndpoint _publishEndpoint;
    public RegisterUserHandler(AuthDbContext db, ITokenService tokenService, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _tokenService = tokenService;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new Exception("User already exists.");
        var hasher = new PasswordHasher<User>();
        var user = new User
        {
            Email = request.Email,
            Username = request.Username
        };
        user.PasswordHash = hasher.HashPassword(user, request.Password);
        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);
        var @event = new UserRegisteredEvent
        {
            UserId = user.Id,
            Username = user.Username,
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
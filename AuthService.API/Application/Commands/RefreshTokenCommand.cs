using AuthService.API.Application.DTOs;
using MediatR;
namespace AuthService.API.Application.Commands;
public record RefreshTokenCommand(string RefreshToken, string Email) : IRequest<UserDto>;
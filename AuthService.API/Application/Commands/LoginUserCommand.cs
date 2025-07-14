using AuthService.API.Application.DTOs;
using MediatR;
namespace AuthService.API.Application.Commands;
public record LoginUserCommand(string Email, string Password) : IRequest<UserDto>;
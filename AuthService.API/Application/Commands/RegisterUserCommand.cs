using AuthService.API.Application.DTOs;
using MediatR;
namespace AuthService.API.Application.Commands;
public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<UserDto>;
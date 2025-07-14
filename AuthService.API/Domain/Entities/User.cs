﻿namespace AuthService.API.Domain.Entities;
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
}
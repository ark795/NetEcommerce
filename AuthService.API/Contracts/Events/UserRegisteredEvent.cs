﻿namespace AuthService.API.Contracts.Events;
public class UserRegisteredEvent
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

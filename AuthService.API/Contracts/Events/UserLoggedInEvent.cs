namespace AuthService.API.Contracts.Events;

public class UserLoggedInEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
}
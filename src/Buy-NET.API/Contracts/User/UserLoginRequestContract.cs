namespace Buy_NET.API.Contracts.User;

public class UserLoginRequestContract
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;      
}
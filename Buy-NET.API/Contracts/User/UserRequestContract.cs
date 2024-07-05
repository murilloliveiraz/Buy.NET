namespace Buy_NET.API.Contracts.User;

public class UserRequestContract : UserLoginRequestContract
{
    public DateTime? InactivationDate { get; set; } 
    public string Role { get; set; }
}
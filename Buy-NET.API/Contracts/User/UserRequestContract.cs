namespace Buy_NET.API.Contracts.User;

public class UserRequestContract : UserLoginRequestContract
{
    public string Role { get; set; }
}
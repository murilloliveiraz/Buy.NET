namespace Buy_NET.API.Contracts.User;

public class UserResponseContract : UserRequestContract
{
    public long Id { get; set; }
    public DateTime RegistrationDate { get; set; } 
    public DateTime? InactivationDate { get; set; } 
}
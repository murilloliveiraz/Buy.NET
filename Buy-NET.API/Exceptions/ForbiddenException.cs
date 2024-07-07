namespace Buy_NET.API.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
}
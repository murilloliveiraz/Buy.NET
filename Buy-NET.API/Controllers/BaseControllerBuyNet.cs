using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

public class BaseControllerBuyNet : ControllerBase
{
    protected long _userId;

    protected long GetLoggedInUser()
    {
        var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        long.TryParse(id, out long userId);
        return userId;
    }

    protected string GetLoggedInUserRole()
    {
        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        return role;
    }
}
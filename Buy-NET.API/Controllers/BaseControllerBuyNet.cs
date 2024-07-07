using System.Security.Claims;
using Buy_NET.API.Contracts.Error;
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

    protected ErrorContract ThrowBadRequest(Exception ex)
        {
            return new ErrorContract{
                StatusCode = 400,
                Title = "Bad Request",
                Description = ex.Message,
                Date = DateTime.Now,
            };
        }

    protected ErrorContract ThrowNotFound(Exception ex)
    {
        return new ErrorContract{
            StatusCode = 404,
            Title = "Not Found",
            Description = ex.Message,
            Date = DateTime.Now,
        };
    }

    protected ErrorContract ThrowUnauthorized(Exception ex)
    {
        return new ErrorContract{
            StatusCode = 401,
            Title = "Unauthorized",
            Description = ex.Message,
            Date = DateTime.Now,
        };
    }
    
    protected ErrorContract ThrowForbidden(Exception ex)
    {
        return new ErrorContract{
            StatusCode = 403,
            Title = "Forbidden",
            Description = ex.Message,
            Date = DateTime.Now,
        };
    }

    
}
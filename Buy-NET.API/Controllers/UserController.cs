using System.Security.Authentication;
using Buy_NET.API.Contracts.User;
using Buy_NET.API.Services.Interfaces.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

[ApiController]
[Route("usuarios")]
public class UserController : ControllerBase
{
    private readonly IUserService  _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequestContract user)
    {
        try
        {
            return Created("", await _userService.Create(user));
        }
        catch (Exception ex)
        {
            
            return Problem(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(UserLoginRequestContract user)
    {
        try
        {
            return Ok(await _userService.Authenticate(user));
        }
        catch(AuthenticationException ex)
        {
            return Unauthorized(new {statusCode = 401, message = ex.Message});
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _userService.Get());
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] long? id, [FromQuery] string email)
    {
        try
        {
            if (id.HasValue)
            {
                return Ok(await _userService.GetById(id.Value));
            }
            else if(!string.IsNullOrEmpty(email))
            {
                return Ok(await _userService.GetByEmail(email));
            }
            else 
            {
                return BadRequest("You must provide either an id or an email.");
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UserRequestContract user)
    {
        try
        {
            return Ok(await _userService.Update(id, user));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _userService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
using System.Security.Authentication;
using Buy_NET.API.Contracts.User;
using Buy_NET.API.Services.Interfaces.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

[ApiController]
[Route("usuarios")]
public class UserController : BaseControllerBuyNet
{
    private readonly IUserService  _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous]
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
    [AllowAnonymous]
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
    [Authorize]
    public async Task<IActionResult> Get()
    {
        try
        {
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                return Unauthorized("Voce não possui permissão para buscar todos os usuários");
            }
            return Ok(await _userService.Get());
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] long? id, [FromQuery] string email)
    {
        try
        {
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                return Unauthorized("Voce não possui permissão para buscar um usuário por nome ou id");
            }
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
    [Authorize]
    public async Task<IActionResult> Update(long id, UserUpdateRequestContract user)
    {
        try
        {
            var userId = GetLoggedInUser();
            var role = GetLoggedInUserRole();
            if (userId != id && role != "Admin")
            {
                return Unauthorized("Voce não pode atualizar o registro de outro usuário");
            }
            return Ok(await _userService.Update(id, user));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                return Unauthorized("Voce não possui permissão para deletar um usuário");
            }
            await _userService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
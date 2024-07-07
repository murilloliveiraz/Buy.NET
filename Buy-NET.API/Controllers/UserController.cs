using System.Security.Authentication;
using Buy_NET.API.Contracts.User;
using Buy_NET.API.Exceptions;
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
        catch(BadRequestException ex)
        {
            return BadRequest(ThrowBadRequest(ex));
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
            return Unauthorized(ThrowUnauthorized(ex));
        }
        catch(BadRequestException ex)
        {
            return BadRequest(ThrowBadRequest(ex));
        }
        catch(NotFoundException ex)
        {
            return NotFound(ThrowNotFound(ex));
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
            long? id = GetLoggedInUser();
            if (id is null || id == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                throw new ForbiddenException("Voce não possui permissão para buscar todos os usuários");
            }
            return Ok(await _userService.Get());
        }
        catch(AuthenticationException ex)
        {
            return Unauthorized(ThrowUnauthorized(ex));
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
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
            long? idLogged = GetLoggedInUser();
            if (idLogged is null || idLogged == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                throw new ForbiddenException("Voce não possui permissão para buscar um usuário por nome ou id");
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
        catch(AuthenticationException ex)
        {
            return Unauthorized(ThrowUnauthorized(ex));
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
        }
        catch(BadRequestException ex)
        {
            return BadRequest(ThrowBadRequest(ex));
        }
        catch(NotFoundException ex)
        {
            return NotFound(ThrowNotFound(ex));
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
            long? idLogged = GetLoggedInUser();
            if (idLogged is null || idLogged == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            var userId = GetLoggedInUser();
            var role = GetLoggedInUserRole();
            if (userId != id && role != "Admin")
            {
                return Unauthorized("Voce não pode atualizar o registro de outro usuário");
            }
            return Ok(await _userService.Update(id, user));
        }
        catch(AuthenticationException ex)
        {
            return Unauthorized(ThrowUnauthorized(ex));
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
        }
        catch(BadRequestException ex)
        {
            return BadRequest(ThrowBadRequest(ex));
        }
        catch(NotFoundException ex)
        {
            return NotFound(ThrowNotFound(ex));
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
            long? idLogged = GetLoggedInUser();
            if (idLogged is null || idLogged == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                return Unauthorized("Voce não possui permissão para deletar um usuário");
            }
            await _userService.Delete(id);
            return NoContent();
        }
        catch(AuthenticationException ex)
        {
            return Unauthorized(ThrowUnauthorized(ex));
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
        }
        catch(BadRequestException ex)
        {
            return BadRequest(ThrowBadRequest(ex));
        }
        catch(NotFoundException ex)
        {
            return NotFound(ThrowNotFound(ex));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
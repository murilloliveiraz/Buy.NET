using System.Security.Authentication;
using Buy_NET.API.Contracts.Category;
using Buy_NET.API.Domain.Exceptions;
using Buy_NET.API.Services.Interfaces.CategoryServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

[ApiController]
[Route("categorias")]
public class CategoryController : BaseControllerBuyNet
{
    private readonly ICategoryService  _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CategoryRequestContract category)
    {
        try
        {
            var role = GetLoggedInUserRole();
            long? id = GetLoggedInUser();
            if (id is null || id == 0)
            {
                throw new UnauthorizedAccessException("É necessário fazer login para ter acesso a esse método");
            }
            if (role != "Admin")
            {
                throw new ForbiddenException("Voce não possui permissão para criar uma categoria");
            }
            return Created("", await _categoryService.Create(category));
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
        }
        catch(UnauthorizedAccessException ex)
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
            return Ok(await _categoryService.Get());
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
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
    
    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] long? id, [FromQuery] string name)
    {
        try
        {
            long? idLogged = GetLoggedInUser();
            if (idLogged is null || idLogged == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            if (id.HasValue)
            {
                return Ok(await _categoryService.GetById(id.Value));
            }
            else if(!string.IsNullOrEmpty(name))
            {
                return Ok(await _categoryService.GetByName(name));
            }
            else 
            {
                return BadRequest("Voce deve informar um nome ou um Id.");
            }
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
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

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(long id, CategoryRequestContract category)
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
                throw new ForbiddenException("Voce não possui permissão para atualizar uma categoria");
            }
            return Ok(await _categoryService.Update(id, category));
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
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
                throw new ForbiddenException("Voce não possui permissão para deletar uma categoria");
            }
            await _categoryService.Delete(id);
            return NoContent();
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(403, ThrowForbidden(ex));
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
}
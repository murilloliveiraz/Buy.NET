using System.Security.Authentication;
using Buy_NET.API.Contracts.Product;
using Buy_NET.API.Domain.Exceptions;
using Buy_NET.API.Services.Interfaces.ProductServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

[ApiController]
[Route("produtos")]
public class ProductController : BaseControllerBuyNet
{
    private readonly IProductService  _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(ProductRequestContract product)
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
                return Unauthorized("Voce não possui permissão para criar um produto");
            }
            return Created("", await _productService.Create(product));
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
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        try
        {
            long? idLogged = GetLoggedInUser();
            if (idLogged is null || idLogged == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            return Ok(await _productService.Get());
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
                return Ok(await _productService.GetById(id.Value));
            }
            else if(!string.IsNullOrEmpty(name))
            {
                return Ok(await _productService.GetByName(name));
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
    public async Task<IActionResult> Update(long id, ProductRequestContract product)
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
                return Unauthorized("Voce não possui permissão para atualizar um produto");
            }
            return Ok(await _productService.Update(id, product));
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
                return Unauthorized("Voce não possui permissão para deletar um produto");
            }
            await _productService.Delete(id);
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
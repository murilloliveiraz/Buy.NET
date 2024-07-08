using System.Security.Authentication;
using Buy_NET.API.Contracts.Order;
using Buy_NET.API.Domain.Exceptions;
using Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

[ApiController]
[Route("pedidos")]
public class OrderController : BaseControllerBuyNet
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(OrderRequestContract order)
    {
        try
        {
            long? id = GetLoggedInUser();
            if (id is null || id == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            if (id != order.CustomerId)
            {
                throw new ForbiddenException("Voce não pode fazer um pedido em nome de outro usuário");
            }
            var createdOrder = await _orderService.Create(order);
        
            var orderResponse = await _orderService.GetById(createdOrder.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, orderResponse);
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
            long? id = GetLoggedInUser();
            if (id is null || id == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                throw new ForbiddenException("Voce não possui permissão para listar todos os pedidos");
            }
            return Ok(await _orderService.Get());
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

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(long id)
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
                throw new ForbiddenException("Voce não possui permissão para buscar por pedidos com um id especifico");
            }
            return Ok(await _orderService.GetById(id));
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

    [HttpGet("meus-pedidos")]
    [Authorize]
    public async Task<IActionResult> GetUserOrders()
    {
        try
        {
            long? id = GetLoggedInUser();
            if (id is null || id == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            long userId = GetLoggedInUser();
            var orders = await _orderService.GetByUserId(userId);
            return Ok(orders);
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

    [HttpGet("meus-pedidos/{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserOrderById(long id)
    {
        try
        {
            long? idLogged = GetLoggedInUser();
            if (idLogged is null || idLogged == 0)
            {
                throw new AuthenticationException("É necessário fazer login para ter acesso a esse método");
            }
            long userId = GetLoggedInUser();
            var order = await _orderService.GetByIdAndUserId(id, userId);
            if (order == null)
            {
                return NotFound("Pedido não encontrado");
            }
            return Ok(order);
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
                return Unauthorized("Voce não possui permissão para deletar um pedido");
            }
            await _orderService.Delete(id);
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
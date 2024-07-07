using Buy_NET.API.Contracts.Order;
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
            var id = GetLoggedInUser();
            if (id != order.CustomerId)
            {
                return Unauthorized("Voce não pode fazer um pedido em nome de outro usuário");
            }
            var createdOrder = await _orderService.Create(order);
        
            var orderResponse = await _orderService.GetById(createdOrder.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, orderResponse);
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
                return Unauthorized("Voce não possui permissão para listar todos os pedidos");
            }
            return Ok(await _orderService.Get());
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
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                return Unauthorized("Voce não possui permissão para buscar por pedidos com um id especifico");
            }
            return Ok(await _orderService.GetById(id));
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
            long userId = GetLoggedInUser();
            var orders = await _orderService.GetByUserId(userId);
            return Ok(orders);
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
            long userId = GetLoggedInUser();
            var order = await _orderService.GetByIdAndUserId(id, userId);
            if (order == null)
            {
                return NotFound("Pedido não encontrado");
            }
            return Ok(order);
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
                return Unauthorized("Voce não possui permissão para deletar um pedido");
            }
            await _orderService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
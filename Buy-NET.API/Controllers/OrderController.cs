using Buy_NET.API.Contracts.Order;
using Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

[ApiController]
[Route("pedidos")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderRequestContract order)
    {
        try
        {
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
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _orderService.Get());
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            return Ok(await _orderService.GetById(id));
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
            await _orderService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
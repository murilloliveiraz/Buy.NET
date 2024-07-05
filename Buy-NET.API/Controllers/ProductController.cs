using Buy_NET.API.Contracts.Product;
using Buy_NET.API.Services.Interfaces.ProductServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buy_NET.API.Controllers;

[ApiController]
[Route("produtos")]
public class ProductController : ControllerBase
{
    private readonly IProductService  _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductRequestContract product)
    {
        try
        {
            return Created("", await _productService.Create(product));
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
            return Ok(await _productService.Get());
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] long? id, [FromQuery] string name)
    {
        try
        {
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
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, ProductRequestContract product)
    {
        try
        {
            return Ok(await _productService.Update(id, product));
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
            await _productService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
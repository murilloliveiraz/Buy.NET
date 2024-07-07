using Buy_NET.API.Contracts.Category;
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
            if (role != "Admin")
            {
                return Unauthorized("Voce não possui permissão para criar uma categoria");
            }
            return Created("", await _categoryService.Create(category));
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
            return Ok(await _categoryService.Get());
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
            var role = GetLoggedInUserRole();
            if (role != "Admin")
            {
                return Unauthorized("Voce não possui permissão para atualizar uma categoria");
            }
            return Ok(await _categoryService.Update(id, category));
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
                return Unauthorized("Voce não possui permissão para deletar uma categoria");
            }
            await _categoryService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
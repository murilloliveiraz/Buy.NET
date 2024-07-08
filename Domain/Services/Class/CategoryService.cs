using AutoMapper;
using Buy_NET.API.Domain.Exceptions;
using Buy_NET.API.Contracts.Category;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.CategoryRepositoryInterface;
using Buy_NET.API.Services.Interfaces.CategoryServiceInterfaces;
using Microsoft.IdentityModel.Tokens;

namespace Buy_NET.API.Services.Class;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponseContract> Create(CategoryRequestContract model)
    {
        if (model is null || model.Name.IsNullOrEmpty())
        {
            throw new BadRequestException("A categoria não pode ser nula");
        }
        Category category = _mapper.Map<Category>(model);
        category.RegistrationDate = DateTime.Now;
        category = await _categoryRepository.Create(category);
        return _mapper.Map<CategoryResponseContract>(category);
    }

    public async Task Delete(long id)
    {
        Category category = await _categoryRepository.GetById(id);
        if (category is null || category.Id != id)
        {
            throw new NotFoundException("Categoria não encontrada");
        }
        await _categoryRepository.Delete(_mapper.Map<Category>(category));
    }

    public async Task<IEnumerable<CategoryResponseContract>> Get()
    {
        var categories = await _categoryRepository.Get();
        if (categories is null )
        {
            throw new NotFoundException("Nenhuma categoria encontrada");
        }
        return categories.Select(c => _mapper.Map<CategoryResponseContract>(c));
    }

    public async Task<CategoryResponseContract> GetById(long id)
    {
        var category = await _categoryRepository.GetById(id);
        if (category is null || category.Id != id)
        {
            throw new NotFoundException("Categoria não encontrada");
        }
        return _mapper.Map<CategoryResponseContract>(category);
    }
    
    public async Task<CategoryResponseContract> GetByName(string name)
    {
        var category = await _categoryRepository.GetByName(name);
        if (category is null || category.Name != name)
        {
            throw new NotFoundException("Categoria não encontrada");
        }
        return _mapper.Map<CategoryResponseContract>(category);
    }

    public async Task<CategoryResponseContract> Update(long id, CategoryRequestContract model)
    {
        Category category = await _categoryRepository.GetById(id);
        if (category is null || category.Id != id)
        {
            throw new NotFoundException("Categoria não encontrada");
        }
        var categoryContract = _mapper.Map<Category>(model);
        categoryContract.Id = category.Id;
        categoryContract.RegistrationDate = category.RegistrationDate;
        categoryContract = await _categoryRepository.Update(categoryContract);
        return _mapper.Map<CategoryResponseContract>(categoryContract);
    }
}
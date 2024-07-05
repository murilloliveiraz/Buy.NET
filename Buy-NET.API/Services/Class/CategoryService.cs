using AutoMapper;
using Buy_NET.API.Contracts.Category;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.CategoryRepositoryInterface;
using Buy_NET.API.Services.Interfaces.CategoryServiceInterfaces;

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
        Category category = _mapper.Map<Category>(model);
        category.RegistrationDate = DateTime.Now;
        category = await _categoryRepository.Create(category);
        return _mapper.Map<CategoryResponseContract>(category);
    }

    public async Task Delete(long id)
    {
        Category expenseCategory = await _categoryRepository.GetById(id);
        await _categoryRepository.Delete(_mapper.Map<Category>(expenseCategory));
    }

    public async Task<IEnumerable<CategoryResponseContract>> Get()
    {
        var categories = await _categoryRepository.Get();
        return categories.Select(c => _mapper.Map<CategoryResponseContract>(c));
    }

    public async Task<CategoryResponseContract> GetById(long id)
    {
        var category = await _categoryRepository.GetById(id);
        return _mapper.Map<CategoryResponseContract>(category);
    }
    
    public async Task<CategoryResponseContract> GetByName(string name)
    {
        var category = await _categoryRepository.GetByName(name);
        return _mapper.Map<CategoryResponseContract>(category);
    }

    public async Task<CategoryResponseContract> Update(long id, CategoryRequestContract model)
    {
        Category category = await _categoryRepository.GetById(id);
        category.Name = model.Name;
        category.Description = model.Description;
        category = await _categoryRepository.Update(category);
        return _mapper.Map<CategoryResponseContract>(category);
    }
}
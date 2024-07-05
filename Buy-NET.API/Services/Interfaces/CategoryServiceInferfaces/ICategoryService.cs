using Buy_NET.API.Contracts.Category;

namespace Buy_NET.API.Services.Interfaces.CategoryServiceInterfaces;

public interface ICategoryService : IService<CategoryRequestContract, CategoryResponseContract, long>
{
    Task<CategoryResponseContract> GetByName(string name);   
}
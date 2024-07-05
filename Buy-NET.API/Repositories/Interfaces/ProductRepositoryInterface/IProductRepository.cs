using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Repositories.Interfaces.ProductRepositoryInterface;

public interface IProductRepository : IRepository<Product, long>
{
    Task<Product?> GetByName(string name);
}
using Buy_NET.API.Contracts.Product;

namespace Buy_NET.API.Services.Interfaces.ProductServiceInterfaces;

public interface IProductService : IService<ProductRequestContract, ProductResponseContract, long>
{
    Task<ProductResponseContract> GetByName(string name);   
}
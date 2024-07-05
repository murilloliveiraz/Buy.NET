using AutoMapper;
using Buy_NET.API.Contracts.Product;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.ProductRepositoryInterface;
using Buy_NET.API.Services.Interfaces.ProductServiceInterfaces;

namespace Buy_NET.API.Services.Class;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductResponseContract> Create(ProductRequestContract model)
    {
        Product Product = _mapper.Map<Product>(model);
        Product.RegistrationDate = DateTime.Now;
        Product = await _productRepository.Create(Product);
        return _mapper.Map<ProductResponseContract>(Product);
    }

    public async Task Delete(long id)
    {
        Product expenseProduct = await _productRepository.GetById(id);
        await _productRepository.Delete(_mapper.Map<Product>(expenseProduct));
    }

    public async Task<IEnumerable<ProductResponseContract>> Get()
    {
        var categories = await _productRepository.Get();
        return categories.Select(c => _mapper.Map<ProductResponseContract>(c));
    }

    public async Task<ProductResponseContract> GetById(long id)
    {
        var Product = await _productRepository.GetById(id);
        return _mapper.Map<ProductResponseContract>(Product);
    }
    
    public async Task<ProductResponseContract> GetByName(string name)
    {
        var Product = await _productRepository.GetByName(name);
        return _mapper.Map<ProductResponseContract>(Product);
    }

    public async Task<ProductResponseContract> Update(long id, ProductRequestContract model)
    {
        Product product = await _productRepository.GetById(id);
        // var productUpdated = _mapper.Map<Product>(model);
        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.StockQuantity = model.StockQuantity;
        product = await _productRepository.Update(product);
        return _mapper.Map<ProductResponseContract>(product);
    }
}
using AutoMapper;
using Buy_NET.API.Contracts.Product;
using Buy_NET.API.Domain.Exceptions;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.CategoryRepositoryInterface;
using Buy_NET.API.Repositories.Interfaces.ProductRepositoryInterface;
using Buy_NET.API.Services.Interfaces.ProductServiceInterfaces;

namespace Buy_NET.API.Services.Class;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductResponseContract> Create(ProductRequestContract model)
    {
        Product Product = _mapper.Map<Product>(model);
        if (model.Price < 0)
        {
            throw new BadRequestException("Os valor do produto não pode ser negativo.");
        }
        if (model.StockQuantity < 0)
        {
            throw new BadRequestException("A quantidade em estoque não pode ser negativa.");
        }
        var categoryExist = _categoryRepository.GetById(model.CategoryId);
        Product.RegistrationDate = DateTime.Now;
        Product = await _productRepository.Create(Product);
        return _mapper.Map<ProductResponseContract>(Product);
    }

    public async Task Delete(long id)
    {
        Product product = await _productRepository.GetById(id);
        if (product is null || product.Id != id)
        {
            throw new NotFoundException("Produto não encontrado");
        }
        await _productRepository.Delete(_mapper.Map<Product>(product));
    }

    public async Task<IEnumerable<ProductResponseContract>> Get()
    {
        var products = await _productRepository.Get();
        if (products is null)
        {
            throw new NotFoundException("Nenhum produto encontrado");
        }
        return products.Select(c => _mapper.Map<ProductResponseContract>(c));
    }

    public async Task<ProductResponseContract> GetById(long id)
    {
        var product = await _productRepository.GetById(id);
        if (product is null || product.Id != id)
        {
            throw new NotFoundException("Produto não encontrado");
        }
        return _mapper.Map<ProductResponseContract>(product);
    }
    
    public async Task<ProductResponseContract> GetByName(string name)
    {
        var product = await _productRepository.GetByName(name);
        if (product is null || product.Name != name)
        {
            throw new NotFoundException("Produto não encontrado");
        }
        return _mapper.Map<ProductResponseContract>(product);
    }

    public async Task<ProductResponseContract> Update(long id, ProductRequestContract model)
    {
        if (model is null)
        {
            throw new BadRequestException("O novo valor do Produto não pode ser nulo");
        }
        Product product = await _productRepository.GetById(id);
        if (product is null || product.Id != id)
        {
            throw new NotFoundException("Produto não encontrado");
        }
        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.StockQuantity = model.StockQuantity;
        product = await _productRepository.Update(product);
        return _mapper.Map<ProductResponseContract>(product);
    }
}
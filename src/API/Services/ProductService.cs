using PubDev.Store.API.Entities;
using PubDev.Store.API.Interfaces.Repositories;
using PubDev.Store.API.Interfaces.Services;
using PubDev.Store.API.Messages;

namespace PubDev.Store.API.Services;

public class ProductService : IProductService
{
    private readonly NotificationContext _notificationContext;
    private readonly IProductRepository _productRepository;

    public ProductService(
        NotificationContext notificationContext,
        IProductRepository productRepository)
    {
        _notificationContext = notificationContext;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetByIdAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is not null)
        {
            return product;
        }

        _notificationContext.AddNotFound(Error.Product.NOT_FOUND, $"Product {productId} not found");

        return null;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        if (product.Value <= 0)
        {
            _notificationContext.AddValidationError(
                Error.Product.INVALID_VALUE, "Product value should be greater than 0");

            return null;
        }

        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        if (product.Value <= 0)
        {
            _notificationContext.AddValidationError(Error.Product.INVALID_VALUE, "Product value should be greater than 0");

            return null;
        }

        var productData = await _productRepository.GetByIdAsync(product.ProductId);

        if (productData is not null)
        {
            return await _productRepository.UpdateAsync(product);
        }

        _notificationContext.AddNotFound(Error.Product.NOT_FOUND, $"Product {product.ProductId} not found");

        return null;
    }
}

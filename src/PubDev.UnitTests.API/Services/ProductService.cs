using PubDev.UnitTests.API.Entities;
using PubDev.UnitTests.API.Enums;
using PubDev.UnitTests.API.Interfaces.Repositories;
using PubDev.UnitTests.API.Interfaces.Services;

namespace PubDev.UnitTests.API.Services;

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

        _notificationContext.AddNotification("PRODUCT_NOT_FOUND", $"Product {productId} not found", ErrorType.NotFound);

        return null;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        if (product.Value <= 0)
        {
            _notificationContext.AddNotification("PRODUCT_VALUE_INVALID", "Product value should be greater than 0", ErrorType.Validation);

            return null;
        }

        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        if (product.Value <= 0)
        {
            _notificationContext.AddNotification("PRODUCT_VALUE_INVALID", "Product value should be greater than 0", ErrorType.Validation);

            return null;
        }

        var productData = await _productRepository.GetByIdAsync(product.ProductId);

        if (productData is not null)
        {
            return await _productRepository.UpdateAsync(product);
        }

        _notificationContext.AddNotification("PRODUCT_NOT_FOUND", $"Product {product.ProductId} not found", ErrorType.Validation);

        return null;
    }
}

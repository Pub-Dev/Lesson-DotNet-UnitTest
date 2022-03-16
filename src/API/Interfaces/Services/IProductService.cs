using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product> GetByIdAsync(int productId);

    Task<Product> CreateAsync(Product product);

    Task<Product> UpdateAsync(Product product);
}
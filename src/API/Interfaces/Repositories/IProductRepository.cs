using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product> GetByIdAsync(int productId);

    Task<Product> CreateAsync(Product product);

    Task<Product> UpdateAsync(Product product);
}
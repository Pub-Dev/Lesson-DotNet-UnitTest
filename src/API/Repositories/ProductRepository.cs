using Dapper;
using PubDev.Store.API.Entities;
using PubDev.Store.API.Interfaces.Repositories;
using System.Data;

namespace PubDev.Store.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnection _dbConnection;

    public ProductRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var data = await _dbConnection.QueryAsync<Product>(
            @"SELECT 
                ProductId,
                Name,
                Value,
                CreateDate
            FROM [dbo].[Product]");

        return data.ToList();
    }

    public async Task<Product> GetByIdAsync(int productId)
    {
        var data = await _dbConnection.QueryFirstOrDefaultAsync<Product>(
            @"SELECT 
                ProductId,
                Name,
                Value,
                CreateDate
            FROM [dbo].[Product]
            WHERE 
                ProductId = @ProductId",
            new { ProductId = productId });

        return data;
    }


    public async Task<Product> CreateAsync(Product product)
    {
        var data = await _dbConnection.QueryFirstAsync<Product>(
           @"INSERT INTO dbo.Product(Name, Value)
                 VALUES(@Name, @Value);
            
                 SELECT
                    ProductId,
                    Name,
                    Value,
                    CreateDate
                FROM [dbo].[Product]
                WHERE 
                    ProductId = SCOPE_IDENTITY()",
           new { product.Name, product.Value });

        return data;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        await _dbConnection.ExecuteAsync(
           @"UPDATE [dbo].[Product]
                 SET
                    Name = @Name,
                    Value = @Value
                 WHERE 
                    ProductId = @ProductId",
           new { product.ProductId, product.Name, product.Value });

        return product;
    }
}

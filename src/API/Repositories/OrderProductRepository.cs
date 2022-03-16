using Dapper;
using PubDev.Store.API.Entities;
using PubDev.Store.API.Interfaces.Repositories;
using System.Data;

namespace PubDev.Store.API.Repositories;

public class OrderProductRepository : IOrderProductRepository
{
    private readonly IDbConnection _dbConnection;

    public OrderProductRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task CreateAsync(int orderId, IEnumerable<OrderProduct> orderProducts)
    {
        await _dbConnection.ExecuteAsync(@"
            INSERT INTO [dbo].[OrderProduct](OrderId, ProductId, Quantity)
            VALUES(@OrderId, @ProductId, @Quantity);",
           orderProducts.Select(x => new
           {
               OrderId = orderId,
               x.ProductId,
               x.Quantity
           }));
    }
}

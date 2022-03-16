using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Interfaces.Repositories;

public interface IOrderProductRepository
{
    Task CreateAsync(int orderId, IEnumerable<OrderProduct> orderProducts);
}
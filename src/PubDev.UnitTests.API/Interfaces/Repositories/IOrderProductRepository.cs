using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Interfaces.Repositories;

public interface IOrderProductRepository
{
    Task CreateAsync(int orderId, IEnumerable<OrderProduct> orderProducts);
}
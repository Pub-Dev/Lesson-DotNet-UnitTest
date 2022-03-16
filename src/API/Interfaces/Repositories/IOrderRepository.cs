using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();

    Task<Order> GetByIdAsync(int orderId);

    Task<Order> CreateAsync(Order order);
}

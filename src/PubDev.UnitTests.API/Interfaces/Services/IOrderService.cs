using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Interfaces.Services;

public interface IOrderService 
{
    Task<IEnumerable<Order>> GetAllAsync();

    Task<Order> GetByIdAsync(int orderId);

    Task<Order> CreateAsync(Order order);
}
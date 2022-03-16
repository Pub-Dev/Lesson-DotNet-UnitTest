using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Interfaces.Services;

public interface IOrderService 
{
    Task<IEnumerable<Order>> GetAllAsync();

    Task<Order> GetByIdAsync(int orderId);

    Task<Order> CreateAsync(Order order);
}
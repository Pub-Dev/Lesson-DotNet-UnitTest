using PubDev.UnitTests.API.Entities;
using PubDev.UnitTests.API.Enums;
using PubDev.UnitTests.API.Interfaces.Repositories;
using PubDev.UnitTests.API.Interfaces.Services;

namespace PubDev.UnitTests.API.Services;

public class OrderService : IOrderService
{
    private readonly NotificationContext _notificationContext;
    private readonly IOrderRepository _orderRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;

    public OrderService(
        NotificationContext notificationContext,
        IOrderRepository orderRepository,
        IClientRepository clientRepository,
        IProductRepository productRepository,
        IOrderProductRepository orderProductRepository)
    {
        _notificationContext = notificationContext;
        _orderRepository = orderRepository;
        _clientRepository = clientRepository;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
    }

    public Task<IEnumerable<Order>> GetAllAsync()
    {
        return _orderRepository.GetAllAsync();
    }

    public async Task<Order> GetByIdAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order is not null)
        {
            return order;
        }

        _notificationContext.AddNotification("ORDER_NOT_FOUND", $"Order {orderId} not found", ErrorType.NotFound);

        return null;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        var client = await _clientRepository.GetByIdAsync(order.ClientId);

        if (client is null)
        {
            _notificationContext.AddNotification("CLIENT_NOT_FOUND", $"Client {order.ClientId} not found", ErrorType.Validation);

            return null;
        }

        if (order.OrderProducts is null || order.OrderProducts.Count == 0)
        {
            _notificationContext.AddNotification("ORDER_EMPTY", $"Order should have at least one product", ErrorType.Validation);

            return null;
        }

        var productDisctinct = order.OrderProducts.Select(x => x.ProductId).Distinct();

        if (productDisctinct.Count() < order.OrderProducts.Count)
        {
            _notificationContext.AddNotification("ORDER_PRODUCT_REPEATED", $"Order should have a list of distict products", ErrorType.Validation);

            return null;
        }

        foreach (var product in order.OrderProducts)
        {
            var productData = await _productRepository.GetByIdAsync(product.ProductId);

            if (productData is null)
            {
                _notificationContext.AddNotification("PRODUCT_NOT_FOUND", $"Product {product.ProductId} not found", ErrorType.Validation);
            }

            if (product.Quantity <= 0)
            {
                _notificationContext
                    .AddNotification("ORDER_PRODUCT_QUANTITY_ZERO", $"Product {product.ProductId} should have the {nameof(product.Quantity)} greater than 0",
                    ErrorType.Validation);
            }
        }

        if (_notificationContext.IsValid)
        {
            var orderData = await _orderRepository.CreateAsync(order);

            await _orderProductRepository.CreateAsync(orderData.OrderId, order.OrderProducts);

            return await _orderRepository.GetByIdAsync(orderData.OrderId);
        }

        return null;
    }
}
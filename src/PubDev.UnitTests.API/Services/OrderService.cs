using PubDev.UnitTests.API.Entities;
using PubDev.UnitTests.API.Interfaces.Repositories;
using PubDev.UnitTests.API.Interfaces.Services;
using PubDev.UnitTests.API.Messages;

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

        _notificationContext.AddNotFound(Error.Order.NOT_FOUND, $"Order {orderId} not found");

        return null;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        var client = await _clientRepository.GetByIdAsync(order.ClientId);

        if (client is null)
        {
            _notificationContext.AddValidationError(Error.Client.NOT_FOUND, $"Client {order.ClientId} not found");

            return null;
        }

        if (order.OrderProducts is null || order.OrderProducts.Count == 0)
        {
            _notificationContext.AddValidationError(Error.Order.EMPTY, $"Order should have at least one product");

            return null;
        }

        var productDisctinct = order.OrderProducts.Select(x => x.ProductId).Distinct();

        if (productDisctinct.Count() < order.OrderProducts.Count)
        {
            _notificationContext.AddValidationError(Error.Order.PRODUCT_REPEATED, $"Order should have a list of distict products");

            return null;
        }

        foreach (var product in order.OrderProducts)
        {
            var productData = await _productRepository.GetByIdAsync(product.ProductId);

            if (productData is null)
            {
                _notificationContext.AddValidationError(Error.Product.NOT_FOUND, $"Product {product.ProductId} not found");
            }

            if (product.Quantity <= 0)
            {
                _notificationContext
                    .AddValidationError(Error.Order.PRODUCT_QUANTITY_ZERO, 
                        $"Product {product.ProductId} should have the {nameof(product.Quantity)} greater than 0");
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
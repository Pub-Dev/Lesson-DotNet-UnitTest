using Moq;
using PubDev.UnitTests.API.Entities;
using PubDev.UnitTests.API.Enums;
using PubDev.UnitTests.API.Interfaces.Repositories;
using PubDev.UnitTests.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PubDev.UnitTests.API.Tests.Services;

public class OrderServiceTest
{
    private readonly NotificationContext _notificationContext = new NotificationContext();
    private readonly Mock<IOrderRepository> _mockOrderRepository = new Mock<IOrderRepository>();
    private readonly Mock<IClientRepository> _mockClientRepository = new Mock<IClientRepository>();
    private readonly Mock<IProductRepository> _mockProductRepository = new Mock<IProductRepository>();
    private readonly Mock<IOrderProductRepository> _mockOrderProductRepository = new Mock<IOrderProductRepository>();

    private OrderService GetOrderService()
    {
        return new OrderService(
            _notificationContext,
            _mockOrderRepository.Object,
            _mockClientRepository.Object,
            _mockProductRepository.Object,
            _mockOrderProductRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_WithValidData_ReturnsOrders()
    {
        // prepare
        _mockOrderRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Order>());

        var service = GetOrderService();

        // act
        var data = await service.GetAllAsync();

        // assert
        Assert.NotNull(data);
        Assert.True(_notificationContext.IsValid);
        _mockOrderRepository.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidData_ReturnsOrder()
    {
        // prepare
        _mockOrderRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Order());

        var service = GetOrderService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.NotNull(data);
        Assert.True(_notificationContext.IsValid);
        _mockOrderRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidData_ReturnsNull()
    {
        // prepare
        _mockOrderRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Order));

        var service = GetOrderService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "ORDER_NOT_FOUND" &&
               x.Message == $"Order 1 not found" &&
               x.ErrorType == ErrorType.NotFound);
        _mockOrderRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidClient_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Client));

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "CLIENT_NOT_FOUND" &&
               x.Message == "Client 1 not found" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithListOfProductNull_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = null
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "ORDER_EMPTY" &&
               x.Message == "Order should have at least one product" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithListOfProductEmpty_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = new List<OrderProduct>()
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "ORDER_EMPTY" &&
               x.Message == "Order should have at least one product" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithListOfProductRepeated_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = new List<OrderProduct>()
            {
                new OrderProduct(){ ProductId = 1, Quantity = 1 },
                new OrderProduct(){ ProductId = 1, Quantity = 1 }
            }
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "ORDER_PRODUCT_REPEATED" &&
               x.Message == "Order should have a list of distict products" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithProductNotFound_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Product));

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = new List<OrderProduct>()
            {
                new OrderProduct(){ ProductId = 1, Quantity = 1 }
            }
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "PRODUCT_NOT_FOUND" &&
               x.Message == "Product 1 not found" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithProductWithInvalidQuantity_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product());

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = new List<OrderProduct>()
            {
                new OrderProduct(){ ProductId = 1, Quantity = -1 }
            }
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "ORDER_PRODUCT_QUANTITY_ZERO" &&
               x.Message == "Product 1 should have the Quantity greater than 0" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithProductNotFoundAndWithInvalidQuantity_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Product));

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = new List<OrderProduct>()
            {
                new OrderProduct(){ ProductId = 1, Quantity = -1 }
            }
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "PRODUCT_NOT_FOUND" &&
               x.Message == "Product 1 not found" &&
               x.ErrorType == ErrorType.Validation);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == "ORDER_PRODUCT_QUANTITY_ZERO" &&
               x.Message == "Product 1 should have the Quantity greater than 0" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithValueData_ReturnsOrder()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product());

        _mockOrderRepository
            .Setup(x => x.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync(new Order());

        var service = GetOrderService();
        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = new List<OrderProduct>()
            {
                new OrderProduct(){ ProductId = 1, Quantity = 1 }
            }
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.True(_notificationContext.IsValid);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Once);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Once);
    }
}

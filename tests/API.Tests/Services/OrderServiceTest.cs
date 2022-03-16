namespace PubDev.Store.API.Tests.Services;

public class OrderServiceTest
{
    private readonly NotificationContext _notificationContext = new();
    private readonly Mock<IOrderRepository> _mockOrderRepository = new();
    private readonly Mock<IClientRepository> _mockClientRepository = new();
    private readonly Mock<IProductRepository> _mockProductRepository = new();
    private readonly Mock<IOrderProductRepository> _mockOrderProductRepository = new();

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
        // arrange
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
        // arrange
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
        // arrange
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
           x => x.ErrorCode == Error.Order.NOT_FOUND &&
               x.Message == $"Order 1 not found" &&
               x.ErrorType == ErrorType.NotFound);
        _mockOrderRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidClient_ReturnsNull()
    {
        // arrange
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
           x => x.ErrorCode == Error.Client.NOT_FOUND &&
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
        // arrange
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
           x => x.ErrorCode == Error.Order.EMPTY &&
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
        // arrange
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
           x => x.ErrorCode == Error.Order.EMPTY &&
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
           x => x.ErrorCode == Error.Order.PRODUCT_REPEATED &&
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
        // arrange
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
           x => x.ErrorCode == Error.Product.NOT_FOUND &&
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
        // arrange
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
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == Error.Order.PRODUCT_QUANTITY_ZERO &&
               x.Message == "Product 1 should have the Quantity greater than 0" &&
               x.ErrorType == ErrorType.Validation);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Never);
        _mockOrderProductRepository.Verify(
            x => x.CreateAsync(It.IsAny<int>(), It.IsAny<IEnumerable<OrderProduct>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithProductNotFoundAndWithInvalidQuantity_ReturnsNull()
    {
        // arrange
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
                new OrderProduct()
                {
                    ProductId = 1,
                    Quantity = -1
                }
            }
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == Error.Product.NOT_FOUND &&
               x.Message == "Product 1 not found" &&
               x.ErrorType == ErrorType.Validation);
        Assert.Contains(_notificationContext.ErrorMessages,
           x => x.ErrorCode == Error.Order.PRODUCT_QUANTITY_ZERO &&
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
        // arrange
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product());

        _mockOrderRepository
            .Setup(x => x.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync(new Order());

        _mockOrderRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Order());

        var service = GetOrderService();

        var order = new Order()
        {
            ClientId = 1,
            OrderProducts = new List<OrderProduct>()
            {
                new OrderProduct()
                {
                    ProductId = 1,
                    Quantity = 1
                }
            }
        };

        // act
        var data = await service.CreateAsync(order);

        // assert
        Assert.NotNull(data);
        Assert.True(_notificationContext.IsValid);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockOrderRepository.Verify(x => x.CreateAsync(It.IsAny<Order>()), Times.Once);
        _mockOrderProductRepository.Verify(x => x.CreateAsync(
            It.IsAny<int>(),
            It.IsAny<IEnumerable<OrderProduct>>()), Times.Once);
    }
}

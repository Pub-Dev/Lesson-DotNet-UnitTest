namespace PubDev.Store.API.Tests.Services;

public class ProductServiceTest
{
    private readonly NotificationContext _notificationContext = new();
    private readonly Mock<IProductRepository> _mockProductRepository = new();

    private ProductService GetProductService()
    {
        return new ProductService(
            _notificationContext,
            _mockProductRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsProduct()
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(new Product());

        var service = GetProductService();
        var product = new Product()
        {
            Name = "test",
            Value = 10
        };

        // act
        var data = await service.CreateAsync(product);

        // assert
        Assert.NotNull(data);
        _mockProductRepository.Verify(x => x.CreateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task CreateAsync_WithInvalidValue_ReturnsError(decimal value)
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(new Product());

        var product = new Product()
        {
            Name = "Product",
            Value = value
        };

        var service = GetProductService();

        // act
        var data = await service.CreateAsync(product);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == Error.Product.INVALID_VALUE &&
                x.Message == "Product value should be greater than 0" &&
                x.ErrorType == ErrorType.Validation);
        _mockProductRepository.Verify(x => x.CreateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_WithValidData_ReturnsProduct()
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Product>());

        var service = GetProductService();

        // act
        var data = await service.GetAllAsync();

        // assert
        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetByIdAsync_WithProductThatDoesNotExist_ReturnsNull()
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Product));

        var service = GetProductService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.Null(data);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == Error.Product.NOT_FOUND &&
                x.Message == $"Product 1 not found" &&
                x.ErrorType == ErrorType.NotFound);
    }

    [Fact]
    public async Task GetByIdAsync_WithProductThatExists_ReturnsProduct()
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product());

        var service = GetProductService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.NotNull(data);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        Assert.True(_notificationContext.IsValid);
    }

    [Fact]
    public async Task UpdateAsync_WithProductThatDoesNotExist_ReturnsNull()
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Product));

        var service = GetProductService();

        var product = new Product()
        {
            ProductId = 1,
            Value = 10
        };

        // act
        var data = await service.UpdateAsync(product);

        // assert
        Assert.Null(data);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == Error.Product.NOT_FOUND &&
                x.Message == $"Product 1 not found" &&
                x.ErrorType == ErrorType.NotFound);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    public static IEnumerable<object[]> InvalidScenarioUpdateProduct = new[] 
    { 
        new object[] 
        {
            new Product()
            {
                ProductId = 1,
                Value = -1
            }
        },
        new object[]
        {
            new Product()
            {
                ProductId = 2,
                Value = -10
            }
        },
        new object[]
        {
            new Product()
            {
                ProductId = 3,
                Value = 0
            }
        }
    };

    [Theory]
    [MemberData(nameof(InvalidScenarioUpdateProduct))]
    public async Task UpdateAsync_WithInvalidValueForProduct_ReturnsNull(Product product)
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Product));

        var service = GetProductService();

        // act
        var data = await service.UpdateAsync(product);

        // assert
        Assert.Null(data);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == Error.Product.INVALID_VALUE &&
                x.Message == $"Product value should be greater than 0" &&
                x.ErrorType == ErrorType.Validation);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _mockProductRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithProductThatExists_ReturnsProduct()
    {
        // arrange
        _mockProductRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product());

        _mockProductRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(new Product());

        var service = GetProductService();

        var product = new Product()
        {
            ProductId = 10,
            Name = "Product",
            Value = 15
        };

        // act
        var data = await service.UpdateAsync(product);

        // assert
        Assert.NotNull(data);
        Assert.True(_notificationContext.IsValid);
        _mockProductRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockProductRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }
}

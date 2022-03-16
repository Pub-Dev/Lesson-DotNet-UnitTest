namespace PubDev.UnitTests.API.Tests.Services;

public class ClientServiceTest
{
    private readonly NotificationContext _notificationContext = new();
    private readonly Mock<IClientRepository> _mockClientRepository = new();

    private ClientService GetClientService()
    {
        return new(
            _notificationContext,
            _mockClientRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsClient()
    {
        // arrange
        _mockClientRepository
            .Setup(x => x.CreateAsync(It.IsAny<Client>()))
            .ReturnsAsync(new Client());

        var service = GetClientService();

        // act
        var data = await service.CreateAsync(new());

        // assert
        Assert.NotNull(data);
        _mockClientRepository.Verify(x => x.CreateAsync(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WithValidData_ReturnClient()
    {
        // arrange
        _mockClientRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Client>());

        var service = GetClientService();

        // act
        var data = await service.GetAllAsync();

        // assert
        Assert.NotNull(data);
        _mockClientRepository.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithClientThatDoesNotExist_ReturnsNull()
    {
        // arrange
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Client));

        var service = GetClientService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.Null(data);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == Error.Client.NOT_FOUND &&
                x.Message == $"Client 1 not found" &&
                x.ErrorType == ErrorType.NotFound);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithClientThatExists_ReturnsClient()
    {
        // arrange
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        var service = GetClientService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.NotNull(data);
        Assert.True(_notificationContext.IsValid);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithClientThatDoesNotExist_ReturnsNull()
    {
        // arrange
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Client));

        var service = GetClientService();

        var client = new Client()
        {
            ClientId = 1
        };

        // act
        var data = await service.UpdateAsync(client);

        // assert
        Assert.Null(data);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == Error.Client.NOT_FOUND &&
                x.Message == $"Client 1 not found" &&
                x.ErrorType == ErrorType.NotFound);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockClientRepository.Verify(x => x.UpdateAsync(It.IsAny<Client>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithClientThatExists_ReturnsClient()
    {
        // arrange
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        _mockClientRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Client>()))
            .ReturnsAsync(new Client());

        var service = GetClientService();

        var client = new Client();

        // act
        var data = await service.UpdateAsync(client);

        // assert
        Assert.NotNull(data);
        Assert.True(_notificationContext.IsValid);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockClientRepository.Verify(x => x.UpdateAsync(It.IsAny<Client>()), Times.Once);
    }
}

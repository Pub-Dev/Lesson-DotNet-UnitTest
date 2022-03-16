using Moq;
using PubDev.UnitTests.API.Entities;
using PubDev.UnitTests.API.Enums;
using PubDev.UnitTests.API.Interfaces.Repositories;
using PubDev.UnitTests.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PubDev.UnitTests.API.Tests.Services;

public class ClientServiceTest
{
    private readonly NotificationContext _notificationContext = new NotificationContext();
    private readonly Mock<IClientRepository> _mockClientRepository = new Mock<IClientRepository>();

    private ClientService GetClientService()
    {
        return new ClientService(
            _notificationContext,
            _mockClientRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsClient()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.CreateAsync(It.IsAny<Client>()))
            .ReturnsAsync(new Client());

        var service = GetClientService();

        // act
        var data = await service.CreateAsync(new Client());

        // assert
        Assert.NotNull(data);
        _mockClientRepository.Verify(x => x.CreateAsync(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WithValidData_ReturnClient()
    {
        // prepare
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
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Client));

        var service = GetClientService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.Null(data);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == "CLIENT_NOT_FOUND" &&
                x.Message == $"Client 1 not found" &&
                x.ErrorType == ErrorType.NotFound);
    }

    [Fact]
    public async Task GetByIdAsync_WithClientThatExists_ReturnsClient()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());

        var service = GetClientService();

        // act
        var data = await service.GetByIdAsync(1);

        // assert
        Assert.NotNull(data);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        Assert.True(_notificationContext.IsValid);
    }

    [Fact]
    public async Task UpdateAsync_WithClientThatDoesNotExist_ReturnsNull()
    {
        // prepare
        _mockClientRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Client));

        var service = GetClientService();
        var client = new Client() { ClientId = 1 };

        // act
        var data = await service.UpdateAsync(client);

        // assert
        Assert.Null(data);
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockClientRepository.Verify(x => x.UpdateAsync(It.IsAny<Client>()), Times.Never);
        Assert.False(_notificationContext.IsValid);
        Assert.Contains(_notificationContext.ErrorMessages,
            x => x.ErrorCode == "CLIENT_NOT_FOUND" &&
                x.Message == $"Client 1 not found" &&
                x.ErrorType == ErrorType.Validation);
    }

    [Fact]
    public async Task UpdateAsync_WithClientThatExists_ReturnsClient()
    {
        // prepare
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
        _mockClientRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _mockClientRepository.Verify(x => x.UpdateAsync(It.IsAny<Client>()), Times.Once);
        Assert.True(_notificationContext.IsValid);
    }
}

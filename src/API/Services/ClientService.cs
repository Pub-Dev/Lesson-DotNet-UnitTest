using PubDev.Store.API.Entities;
using PubDev.Store.API.Interfaces.Repositories;
using PubDev.Store.API.Interfaces.Services;
using PubDev.Store.API.Messages;

namespace PubDev.Store.API.Services;

public class ClientService : IClientService
{
    private readonly NotificationContext _notificationContext;
    private readonly IClientRepository _clientRepository;

    public ClientService(
        NotificationContext notificationContext,
        IClientRepository clientRepository)
    {
        _notificationContext = notificationContext;
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client> GetByIdAsync(int clientId)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is not null)
        {
            return client;
        }

        _notificationContext.AddNotFound(Error.Client.NOT_FOUND, $"Client {clientId} not found");

        return null;
    }

    public async Task<Client> CreateAsync(Client client)
    {
        return await _clientRepository.CreateAsync(client);
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        var clientData = await _clientRepository.GetByIdAsync(client.ClientId);

        if (clientData is not null)
        {
            return await _clientRepository.UpdateAsync(client);
        }

        _notificationContext.AddNotFound(Error.Client.NOT_FOUND, $"Client {client.ClientId} not found");

        return null;
    }

    public int Sum(int a, int b)
    {
        return a + b;
    }
}

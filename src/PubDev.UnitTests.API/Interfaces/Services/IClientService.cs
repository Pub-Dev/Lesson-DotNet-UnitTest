using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Interfaces.Services;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllAsync();

    Task<Client> GetByIdAsync(int clientId);

    Task<Client> CreateAsync(Client client);

    Task<Client> UpdateAsync(Client client);
}

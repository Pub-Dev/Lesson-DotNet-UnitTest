using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Interfaces.Services;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllAsync();

    Task<Client> GetByIdAsync(int clientId);

    Task<Client> CreateAsync(Client client);

    Task<Client> UpdateAsync(Client client);
}

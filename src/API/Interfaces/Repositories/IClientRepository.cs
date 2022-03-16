using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Interfaces.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();

    Task<Client> GetByIdAsync(int clientId);

    Task<Client> CreateAsync(Client client);

    Task<Client> UpdateAsync(Client client);
}

using Dapper;
using PubDev.UnitTests.API.Entities;
using PubDev.UnitTests.API.Interfaces.Repositories;
using System.Data;

namespace PubDev.UnitTests.API.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IDbConnection _dbConnection;

    public ClientRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        var data = await _dbConnection.QueryAsync<Client>(
            @"SELECT
                ClientId,
                Name,
                CreateDate
            FROM [dbo].[Client]");

        return data.ToList();
    }

    public async Task<Client> GetByIdAsync(int clientId)
    {
        var data = await _dbConnection.QueryFirstOrDefaultAsync<Client>(
            @"SELECT
                    ClientId,
                    Name,
                    CreateDate
                FROM [dbo].[Client]
                WHERE
                    ClientId = @ClientId",
            new { ClientId = clientId });

        return data;
    }

    public async Task<Client> CreateAsync(Client client)
    {
        var data = await _dbConnection.QueryFirstAsync<Client>(
           @"INSERT INTO [dbo].[Client](Name)
                 VALUES(@Name);

                 SELECT
                    ClientId,
                    Name,
                    CreateDate
                FROM dbo.Client
                WHERE
                    ClientId = SCOPE_IDENTITY()",
           new { client.Name });

        return data;
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        await _dbConnection.ExecuteAsync(
           @"UPDATE [dbo].[Client]
                 SET
                    Name = @Name
                 WHERE
                    ClientId = @ClientId",
           new { client.ClientId, client.Name });

        return client;
    }
}

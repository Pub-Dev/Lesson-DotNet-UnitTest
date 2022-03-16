using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Requests;

public class ClientCreateRequest
{
    public string Name { get; set; }
    public string Address { get; set; }

    public static explicit operator Client(ClientCreateRequest clientCreateRequest)
    {
        return new()
        {
            Name = clientCreateRequest.Name
        };
    }
}

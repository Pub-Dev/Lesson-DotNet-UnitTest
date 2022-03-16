using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Requests;

public class ClientPatchRequest
{
    public string Name { get; set; }

    public static explicit operator Client(ClientPatchRequest clientPatchRequest)
    {
        return new()
        {
            Name = clientPatchRequest.Name
        };
    }
}


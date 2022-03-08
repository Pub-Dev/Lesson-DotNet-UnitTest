using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Requests;

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


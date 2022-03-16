using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Requests;

public class ProductPatchRequest
{
    public string Name { get; set; }
    public decimal Value { get; set; }

    public static explicit operator Product(ProductPatchRequest productPatchRequest)
    {
        return new()
        {
            Name = productPatchRequest.Name,
            Value = productPatchRequest.Value
        };
    }
}

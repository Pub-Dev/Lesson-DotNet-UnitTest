using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Requests;

public class ProductCreateRequest
{
    public string Name { get; set; }
    public decimal Value { get; set; }

    public static explicit operator Product(ProductCreateRequest productCreateRequest)
    {
        return new()
        {
            Name = productCreateRequest.Name,
            Value = productCreateRequest.Value
        };
    }
}
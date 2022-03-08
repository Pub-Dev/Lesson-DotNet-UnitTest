using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Responses;

public class ProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }

    public static explicit operator ProductResponse(Product product)
    {
        return new()
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Value = product.Value,
        };
    }
}

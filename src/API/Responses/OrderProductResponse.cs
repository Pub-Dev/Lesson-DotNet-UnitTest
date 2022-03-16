using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Responses;

public class OrderProductResponse
{
    public ProductResponse Product { get; set; }
    public int Quantity { get; set; }
    public decimal Value => Quantity * Product.Value;

    public static explicit operator OrderProductResponse(OrderProduct orderProduct)
    {
        return new()
        {
            Product = (ProductResponse)orderProduct.Product,
            Quantity = orderProduct.Quantity,
        };
    }
}
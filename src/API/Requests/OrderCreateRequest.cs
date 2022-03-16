using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Requests;

public class OrderCreateRequest
{
    public int ClientId { get; set; }
    public OrderProductRequest[] Products { get; set; }

    public static explicit operator Order(OrderCreateRequest orderCreateRequest)
    {
        return new()
        {
            ClientId = orderCreateRequest.ClientId,
            OrderProducts = orderCreateRequest.Products.Select(x => (OrderProduct)x).ToList()
        };
    }
}

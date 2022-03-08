using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Responses;

public class OrderGetByIdResponse
{
    public int OrderId { get; set; }
    public ClientResponse Client { get; set; }
    public OrderProductResponse[] Items { get; set; }
    public decimal Total => Items.Sum(x => x.Value);
    public DateTime CreatedDate { get; set; }

    public static explicit operator OrderGetByIdResponse(Order order)
    {
        return new()
        {
            OrderId = order.OrderId,
            Client = (ClientResponse)order.Client,
            Items = order.OrderProducts.Select(x => (OrderProductResponse)x).ToArray(),
            CreatedDate = order.CreateDate
        };
    }
}

namespace PubDev.UnitTests.API.Entities;

public class OrderProduct : BaseEntity
{
    public int OrderProductId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
}

namespace PubDev.UnitTests.API.Entities;

public class Order : BaseEntity
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
}

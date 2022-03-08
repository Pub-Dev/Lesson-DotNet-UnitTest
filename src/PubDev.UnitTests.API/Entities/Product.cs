namespace PubDev.UnitTests.API.Entities;

public class Product : BaseEntity
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public ICollection<OrderProduct> OrderProducts = new HashSet<OrderProduct>();
}

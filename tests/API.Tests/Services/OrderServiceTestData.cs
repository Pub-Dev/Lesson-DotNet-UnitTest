namespace PubDev.Store.API.Tests.Services
{
    public static class OrderServiceTestData
    {
        public static IEnumerable<object[]> WithProductNotFoundAndWithInvalidQuantity = new[]
        {
            new object[]
            {
                new Order()
                {
                    ClientId = 1,
                    OrderProducts = new List<OrderProduct>()
                    {
                        new OrderProduct()
                        {
                            ProductId = 1,
                            Quantity = -1
                        }
                    }
                }
            },
            new object[]
            {
                new Order()
                {
                    ClientId = 1,
                    OrderProducts = new List<OrderProduct>()
                    {
                        new OrderProduct()
                        {
                            ProductId = 1,
                            Quantity = 0
                        }
                    }
                }
            }
        };
    }
}

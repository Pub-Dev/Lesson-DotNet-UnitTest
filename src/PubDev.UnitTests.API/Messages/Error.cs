namespace PubDev.UnitTests.API.Messages
{
    public static class Error
    {
        public static class Product        
        {
            public const string NOT_FOUND = "PRODUCT_NOT_FOUND";
            public const string INVALID_VALUE = "PRODUCT_INVALID_VALUE";            
        }

        public static class Order
        {
            public const string NOT_FOUND = "ORDER_NOT_FOUND";
            public const string EMPTY = "ORDER_EMPTY";
            public const string PRODUCT_REPEATED = "ORDER_PRODUCT_REPEATED";
            public const string PRODUCT_QUANTITY_ZERO = "ORDER_PRODUCT_QUANTITY_ZERO";
        }

        public static class Client
        {
            public const string NOT_FOUND = "CLIENT_NOT_FOUND";
        }
    }
}

﻿using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Responses;

public class ProductCreateResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedDate { get; set; }

    public static explicit operator ProductCreateResponse(Product product)
    {
        return new()
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Value = product.Value,
            CreatedDate = product.CreateDate
        };
    }
}
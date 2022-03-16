using Microsoft.AspNetCore.Mvc;
using PubDev.Store.API.Entities;
using PubDev.Store.API.Interfaces.Presenters;
using PubDev.Store.API.Interfaces.Services;
using PubDev.Store.API.Requests;
using PubDev.Store.API.Responses;
using System.Net;

namespace PubDev.Store.API.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly IPresenter _presenter;
    private readonly IProductService _productService;

    public ProductController(
        IPresenter presenter,
        IProductService productService)
    {
        _presenter = presenter;
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductGetAllResponse[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductAllAsync()
    {
        var data = await _productService.GetAllAsync();

        return _presenter.GetResult(data, data => data.Select(x => (ProductGetAllResponse)x).ToArray());
    }

    [HttpGet("{productId}")]
    [ActionName(nameof(GetProductByIdAsync))]
    [ProducesResponseType(typeof(ProductGetByIdResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductByIdAsync(int productId)
    {
        var data = await _productService.GetByIdAsync(productId);

        return _presenter.GetResult(data, data => (ProductGetByIdResponse)data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductCreateResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateProductAsync(ProductCreateRequest request)
    {
        var client = (Product)request;

        var data = await _productService.CreateAsync(client);

        return _presenter.CreateResult(
            data,
            data => (ProductCreateResponse)data, (data) =>
            (nameof(GetProductByIdAsync), "Product", new { data.ProductId }));
    }

    [HttpPatch("{productId}")]
    [ProducesResponseType(typeof(ProductPatchResponse), (int)HttpStatusCode.Accepted)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PatchProductAsync(int productId, ProductPatchRequest request)
    {
        var product = (Product)request;

        product.ProductId = productId;

        var data = await _productService.UpdateAsync(product);

        return _presenter.AcceptedResult(
            data,
            data => (ProductPatchResponse)data, (data) =>
            (nameof(GetProductByIdAsync), "Product", new { data.ProductId }));
    }
}


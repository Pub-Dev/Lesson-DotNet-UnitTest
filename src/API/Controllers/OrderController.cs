﻿using Microsoft.AspNetCore.Mvc;
using PubDev.Store.API.Entities;
using PubDev.Store.API.Interfaces.Presenters;
using PubDev.Store.API.Interfaces.Services;
using PubDev.Store.API.Requests;
using PubDev.Store.API.Responses;
using System.Net;

namespace PubDev.Store.API.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly IPresenter _presenter;
    private readonly IOrderService _orderService;

    public OrderController(
        IPresenter presenter,
        IOrderService orderService)
    {
        _presenter = presenter;
        _orderService = orderService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(OrderGetByIdResponse[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOrderAllAsync()
    {
        var data = await _orderService.GetAllAsync();

        return _presenter.GetResult(data, data => data.Select(order => (OrderGetByIdResponse)order).ToArray());
    }


    [HttpGet("{orderId}")]
    [ActionName(nameof(GetOrderByIdAsync))]
    [ProducesResponseType(typeof(OrderGetByIdResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOrderByIdAsync(int orderId)
    {
        var data = await _orderService.GetByIdAsync(orderId);

        return _presenter.GetResult(data, data => (OrderGetByIdResponse)data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderCreateResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateOrderAsync(OrderCreateRequest request)
    {
        var order = (Order)request;

        var data = await _orderService.CreateAsync(order);

        return _presenter.CreateResult(
            data,
            data => (OrderCreateResponse)data, (data) =>
            (nameof(GetOrderByIdAsync), "Order", new { data.OrderId }));
    }
}



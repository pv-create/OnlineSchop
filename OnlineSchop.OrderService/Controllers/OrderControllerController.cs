using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OnlineSchop.Contracts.Events.Contracts.Events;

namespace OnlineSchop.OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

    public OrderController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var orderId = Guid.NewGuid();

        await _publishEndpoint.Publish(new OrderStarted
        {
            OrderId = orderId,
            ClientId = request.ClientId,
            TotalAmount = request.TotalAmount,
            OrderDate = DateTime.UtcNow
        });

        return Ok(new { OrderId = orderId });
    }
    }

    public class CreateOrderRequest
    {
        public Guid ClientId { get; set; }
        public int TotalAmount { get; set; }
    }
}
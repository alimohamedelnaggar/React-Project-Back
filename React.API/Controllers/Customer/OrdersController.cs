using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using React.BLL.Common;
using React.BLL.DTOs.Order;
using React.BLL.Interfaces;

namespace React.API.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _orderService.CreateOrderAsync(userId, createOrderDto);
        return Ok(response);
    }

    [HttpGet("my-orders")]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetMyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _orderService.GetMyOrdersAsync(userId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _orderService.GetOrderByIdAsync(id, userId);
        return Ok(response);
    }
}

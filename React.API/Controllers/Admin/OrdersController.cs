using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using React.BLL.Common;
using React.BLL.DTOs.Order;
using React.BLL.Interfaces;

namespace React.API.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetAllOrders()
    {
        var response = await _orderService.GetAllOrdersAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
    {
        var response = await _orderService.GetOrderByIdForAdminAsync(id);
        return Ok(response);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
    {
        var response = await _orderService.UpdateOrderStatusAsync(id, dto.Status);
        return Ok(response);
    }
}

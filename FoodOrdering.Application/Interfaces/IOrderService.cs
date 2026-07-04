using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Order;

namespace FoodOrdering.Application.Interfaces;

public interface IOrderService
{
    Task<ApiResponse<OrderDto>> CreateOrderAsync(string userId, CreateOrderDto createOrderDto);
    Task<ApiResponse<IEnumerable<OrderDto>>> GetMyOrdersAsync(string userId);
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int id, string userId);
    Task<ApiResponse<OrderDto>> GetOrderByIdForAdminAsync(int id);
    Task<ApiResponse<IEnumerable<OrderDto>>> GetAllOrdersAsync();
    Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int id, string status);
}

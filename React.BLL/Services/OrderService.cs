using AutoMapper;
using React.BLL.Common;
using React.BLL.DTOs.Order;
using React.BLL.Interfaces;
using React.DAL.Entities;
using React.DAL.Enums;
using React.DAL.Exceptions;
using React.DAL.Interfaces;

namespace React.BLL.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<OrderDto>> CreateOrderAsync(string userId, CreateOrderDto createOrderDto)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        if (cart == null || !cart.CartItems.Any())
            throw new BadRequestException("Cart is empty");

        foreach (var cartItem in cart.CartItems)
        {
            var meal = await _unitOfWork.Meals.GetByIdAsync(cartItem.MealId);
            if (meal == null)
                throw new NotFoundException(nameof(Meal), cartItem.MealId);

            if (meal.Quantity < cartItem.Quantity)
                throw new BadRequestException($"Insufficient stock for {meal.Name}. Available: {meal.Quantity}");

            meal.Quantity -= cartItem.Quantity;
            _unitOfWork.Meals.Update(meal);
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            ShippingAddress = createOrderDto.ShippingAddress,
            TotalPrice = cart.CartItems.Sum(ci => ci.Meal!.Price * ci.Quantity),
            OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                MealId = ci.MealId,
                Quantity = ci.Quantity,
                Price = ci.Meal!.Price
            }).ToList()
        };

        await _unitOfWork.Orders.AddAsync(order);

        _unitOfWork.Carts.ClearCartItems(cart.CartItems.ToList());

        await _unitOfWork.CompleteAsync();

        var orderDto = await MapToOrderDto(order.Id);
        return ApiResponse<OrderDto>.SuccessResult(orderDto!, "Order placed successfully");
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetMyOrdersAsync(string userId)
    {
        var orders = await _unitOfWork.Orders.GetOrdersByUserAsync(userId);
        var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
        return ApiResponse<IEnumerable<OrderDto>>.SuccessResult(orderDtos);
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int id, string userId)
    {
        var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(id);
        if (order == null)
            throw new NotFoundException(nameof(Order), id);

        if (order.UserId != userId)
            throw new BadRequestException("Access denied");

        var orderDto = _mapper.Map<OrderDto>(order);
        return ApiResponse<OrderDto>.SuccessResult(orderDto);
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdForAdminAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(id);
        if (order == null)
            throw new NotFoundException(nameof(Order), id);

        var orderDto = _mapper.Map<OrderDto>(order);
        return ApiResponse<OrderDto>.SuccessResult(orderDto);
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllOrdersWithDetailsAsync();
        var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
        return ApiResponse<IEnumerable<OrderDto>>.SuccessResult(orderDtos);
    }

    public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int id, string status)
    {
        var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(id);
        if (order == null)
            throw new NotFoundException(nameof(Order), id);

        if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            throw new BadRequestException($"Invalid status. Valid values: {string.Join(", ", Enum.GetNames<OrderStatus>())}");

        order.Status = orderStatus;
        _unitOfWork.Orders.Update(order);
        await _unitOfWork.CompleteAsync();

        var orderDto = _mapper.Map<OrderDto>(order);
        return ApiResponse<OrderDto>.SuccessResult(orderDto, "Order status updated");
    }

    private async Task<OrderDto?> MapToOrderDto(int orderId)
    {
        var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(orderId);
        return order == null ? null : _mapper.Map<OrderDto>(order);
    }
}

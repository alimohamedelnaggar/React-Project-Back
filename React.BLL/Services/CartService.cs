using AutoMapper;
using React.BLL.Common;
using React.BLL.DTOs.Cart;
using React.BLL.Interfaces;
using React.DAL.Entities;
using React.DAL.Exceptions;
using React.DAL.Interfaces;

namespace React.BLL.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CartService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CartDto>> GetCartAsync(string userId)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.CompleteAsync();
            cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        }

        var cartDto = MapToCartDto(cart!);
        return ApiResponse<CartDto>.SuccessResult(cartDto);
    }

    public async Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto addToCartDto)
    {
        var meal = await _unitOfWork.Meals.GetByIdAsync(addToCartDto.MealId);
        if (meal == null)
            throw new NotFoundException(nameof(Meal), addToCartDto.MealId);

        if (meal.Quantity < addToCartDto.Quantity)
            throw new BadRequestException($"Only {meal.Quantity} items available in stock");

        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.CompleteAsync();
        }

        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.MealId == addToCartDto.MealId);
        if (existingItem != null)
        {
            var newQuantity = existingItem.Quantity + addToCartDto.Quantity;
            if (newQuantity > meal.Quantity)
                throw new BadRequestException($"Only {meal.Quantity} items available in stock");
            existingItem.Quantity = newQuantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                MealId = addToCartDto.MealId,
                Quantity = addToCartDto.Quantity
            };

            cart.CartItems.Add(cartItem);
        }

        await _unitOfWork.CompleteAsync();

        cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        var cartDto = MapToCartDto(cart!);
        return ApiResponse<CartDto>.SuccessResult(cartDto, "Item added to cart");
    }

    public async Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto updateDto)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        if (cart == null)
            throw new BadRequestException("Cart not found");

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == updateDto.CartItemId);
        if (cartItem == null)
            throw new NotFoundException(nameof(CartItem), updateDto.CartItemId);

        var meal = await _unitOfWork.Meals.GetByIdAsync(cartItem.MealId);
        if (meal == null)
            throw new NotFoundException(nameof(Meal), cartItem.MealId);

        if (updateDto.Quantity > meal.Quantity)
            throw new BadRequestException($"Only {meal.Quantity} items available in stock");

        if (updateDto.Quantity <= 0)
        {
            _unitOfWork.Carts.DeleteCartItem(cartItem);
        }
        else
        {
            cartItem.Quantity = updateDto.Quantity;
        }

        await _unitOfWork.CompleteAsync();

        cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        var cartDto = MapToCartDto(cart!);
        return ApiResponse<CartDto>.SuccessResult(cartDto, "Cart updated successfully");
    }

    public async Task<ApiResponse> RemoveFromCartAsync(string userId, int cartItemId)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        if (cart == null)
            throw new BadRequestException("Cart not found");

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem == null)
            throw new NotFoundException(nameof(CartItem), cartItemId);

        _unitOfWork.Carts.DeleteCartItem(cartItem);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.SuccessResult("Item removed from cart");
    }

    public async Task<ApiResponse> ClearCartAsync(string userId)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        if (cart == null)
            throw new BadRequestException("Cart not found");

        _unitOfWork.Carts.ClearCartItems(cart.CartItems.ToList());
        await _unitOfWork.CompleteAsync();
        return ApiResponse.SuccessResult("Cart cleared successfully");
    }

    private CartDto MapToCartDto(Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.CartItems.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                MealId = ci.MealId,
                MealName = ci.Meal?.Name ?? "",
                MealImageUrl = ci.Meal?.ImageUrl ?? "",
                MealPrice = ci.Meal?.Price ?? 0,
                Quantity = ci.Quantity
            }).ToList()
        };
    }
}

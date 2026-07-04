using React.BLL.Common;
using React.BLL.DTOs.Cart;

namespace React.BLL.Interfaces;

public interface ICartService
{
    Task<ApiResponse<CartDto>> GetCartAsync(string userId);
    Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto addToCartDto);
    Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto updateDto);
    Task<ApiResponse> RemoveFromCartAsync(string userId, int cartItemId);
    Task<ApiResponse> ClearCartAsync(string userId);
}

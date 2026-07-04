using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Auth;

namespace FoodOrdering.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
}

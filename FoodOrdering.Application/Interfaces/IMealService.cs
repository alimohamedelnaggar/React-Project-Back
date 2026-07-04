using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Meal;

namespace FoodOrdering.Application.Interfaces;

public interface IMealService
{
    Task<ApiResponse<PagedResponse<MealDto>>> GetAllAsync(int pageIndex = 1, int pageSize = 10, string? search = null, int? categoryId = null);
    Task<ApiResponse<MealDto>> GetByIdAsync(int id);
    Task<ApiResponse<MealDto>> CreateAsync(CreateMealDto createDto);
    Task<ApiResponse<MealDto>> UpdateAsync(UpdateMealDto updateDto);
    Task<ApiResponse> DeleteAsync(int id);
}

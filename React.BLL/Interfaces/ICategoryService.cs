using React.BLL.Common;
using React.BLL.DTOs.Category;

namespace React.BLL.Interfaces;

public interface ICategoryService
{
    Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync();
    Task<ApiResponse<CategoryDto>> GetByIdAsync(int id);
    Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryDto createDto);
    Task<ApiResponse<CategoryDto>> UpdateAsync(UpdateCategoryDto updateDto);
    Task<ApiResponse> DeleteAsync(int id);
}

using AutoMapper;
using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Category;
using FoodOrdering.Application.Interfaces;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Domain.Exceptions;
using FoodOrdering.Domain.Interfaces;

namespace FoodOrdering.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        return ApiResponse<IEnumerable<CategoryDto>>.SuccessResult(categoryDtos);
    }

    public async Task<ApiResponse<CategoryDto>> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetCategoryWithMealsAsync(id);
        if (category == null)
            throw new NotFoundException(nameof(Category), id);

        var categoryDto = _mapper.Map<CategoryDto>(category);
        categoryDto.MealsCount = category.Meals.Count;

        return ApiResponse<CategoryDto>.SuccessResult(categoryDto);
    }

    public async Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryDto createDto)
    {
        var category = _mapper.Map<Category>(createDto);
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return ApiResponse<CategoryDto>.SuccessResult(categoryDto, "Category created successfully");
    }

    public async Task<ApiResponse<CategoryDto>> UpdateAsync(UpdateCategoryDto updateDto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(updateDto.Id);
        if (category == null)
            throw new NotFoundException(nameof(Category), updateDto.Id);

        _mapper.Map(updateDto, category);
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.CompleteAsync();

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return ApiResponse<CategoryDto>.SuccessResult(categoryDto, "Category updated successfully");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            throw new NotFoundException(nameof(Category), id);

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.SuccessResult("Category deleted successfully");
    }
}

using AutoMapper;
using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Meal;
using FoodOrdering.Application.Interfaces;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Domain.Exceptions;
using FoodOrdering.Domain.Interfaces;

namespace FoodOrdering.Application.Services;

public class MealService : IMealService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MealService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResponse<MealDto>>> GetAllAsync(
        int pageIndex = 1, int pageSize = 10, string? search = null, int? categoryId = null)
    {
        var (items, totalCount) = await _unitOfWork.Meals
            .GetPagedAsync(pageIndex, pageSize, search, categoryId);

        var mealDtos = _mapper.Map<IEnumerable<MealDto>>(items);

        var response = new PagedResponse<MealDto>
        {
            Items = mealDtos,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return ApiResponse<PagedResponse<MealDto>>.SuccessResult(response);
    }

    public async Task<ApiResponse<MealDto>> GetByIdAsync(int id)
    {
        var meal = await _unitOfWork.Meals.GetMealWithCategoryAsync(id);
        if (meal == null)
            throw new NotFoundException(nameof(Meal), id);

        var mealDto = _mapper.Map<MealDto>(meal);
        return ApiResponse<MealDto>.SuccessResult(mealDto);
    }

    public async Task<ApiResponse<MealDto>> CreateAsync(CreateMealDto createDto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(createDto.CategoryId);
        if (category == null)
            throw new NotFoundException(nameof(Category), createDto.CategoryId);

        var meal = _mapper.Map<Meal>(createDto);
        await _unitOfWork.Meals.AddAsync(meal);
        await _unitOfWork.CompleteAsync();

        var mealDto = _mapper.Map<MealDto>(meal);
        return ApiResponse<MealDto>.SuccessResult(mealDto, "Meal created successfully");
    }

    public async Task<ApiResponse<MealDto>> UpdateAsync(UpdateMealDto updateDto)
    {
        var meal = await _unitOfWork.Meals.GetMealWithCategoryAsync(updateDto.Id);
        if (meal == null)
            throw new NotFoundException(nameof(Meal), updateDto.Id);

        var category = await _unitOfWork.Categories.GetByIdAsync(updateDto.CategoryId);
        if (category == null)
            throw new NotFoundException(nameof(Category), updateDto.CategoryId);

        _mapper.Map(updateDto, meal);
        _unitOfWork.Meals.Update(meal);
        await _unitOfWork.CompleteAsync();

        var mealDto = _mapper.Map<MealDto>(meal);
        return ApiResponse<MealDto>.SuccessResult(mealDto, "Meal updated successfully");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var meal = await _unitOfWork.Meals.GetByIdAsync(id);
        if (meal == null)
            throw new NotFoundException(nameof(Meal), id);

        _unitOfWork.Meals.Delete(meal);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.SuccessResult("Meal deleted successfully");
    }
}

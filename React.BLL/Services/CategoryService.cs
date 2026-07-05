using AutoMapper;
using React.BLL.Common;
using React.BLL.DTOs.Category;
using React.BLL.Interfaces;
using React.DAL.Entities;
using React.DAL.Exceptions;
using React.DAL.Interfaces;

namespace React.BLL.Services;

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

    public async Task<ApiResponse<PagedResponse<CategoryDto>>> GetAllAsync(int pageIndex, int pageSize)
    {
        var (items, totalCount) = await _unitOfWork.Categories.GetPagedAsync(pageIndex, pageSize);
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(items);
        var response = new PagedResponse<CategoryDto>
        {
            Items = categoryDtos,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount
        };
        return ApiResponse<PagedResponse<CategoryDto>>.SuccessResult(response);
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

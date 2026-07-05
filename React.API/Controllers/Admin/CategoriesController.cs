using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using React.BLL.Common;
using React.BLL.DTOs.Category;
using React.BLL.Interfaces;

namespace React.API.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<CategoryDto>>>> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _categoryService.GetAllAsync(pageIndex, pageSize);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(int id)
    {
        var response = await _categoryService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Create([FromBody] CreateCategoryDto createDto)
    {
        var response = await _categoryService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Update(int id, [FromBody] UpdateCategoryDto updateDto)
    {
        updateDto.Id = id;
        var response = await _categoryService.UpdateAsync(updateDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var response = await _categoryService.DeleteAsync(id);
        return Ok(response);
    }
}

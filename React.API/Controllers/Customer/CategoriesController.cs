using Microsoft.AspNetCore.Mvc;
using React.BLL.Common;
using React.BLL.DTOs.Category;
using React.BLL.Interfaces;

namespace React.API.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
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
}

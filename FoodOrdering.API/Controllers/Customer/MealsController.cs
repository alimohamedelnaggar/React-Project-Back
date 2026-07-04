using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Meal;
using FoodOrdering.Application.Interfaces;

namespace FoodOrdering.API.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MealsController : ControllerBase
{
    private readonly IMealService _mealService;

    public MealsController(IMealService mealService)
    {
        _mealService = mealService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PagedResponse<MealDto>>>> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null)
    {
        var response = await _mealService.GetAllAsync(pageIndex, pageSize, search, categoryId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<MealDto>>> GetById(int id)
    {
        var response = await _mealService.GetByIdAsync(id);
        return Ok(response);
    }
}

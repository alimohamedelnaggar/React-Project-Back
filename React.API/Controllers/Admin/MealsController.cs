using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using React.BLL.Common;
using React.BLL.DTOs.Meal;
using React.BLL.Interfaces;

namespace React.API.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class MealsController : ControllerBase
{
    private readonly IMealService _mealService;

    public MealsController(IMealService mealService)
    {
        _mealService = mealService;
    }

    [HttpGet]
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
    public async Task<ActionResult<ApiResponse<MealDto>>> GetById(int id)
    {
        var response = await _mealService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost("upload")]
    public async Task<ActionResult<ApiResponse<string>>> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse<string>.FailureResult("No file uploaded"));

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            return BadRequest(ApiResponse<string>.FailureResult("Invalid file format. Allowed: jpg, jpeg, png, gif, webp"));

        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "meals");
        var filePath = Path.Combine(uploadsDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var url = $"/uploads/meals/{fileName}";
        return Ok(ApiResponse<string>.SuccessResult(url, "Image uploaded successfully"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MealDto>>> Create([FromBody] CreateMealDto createDto)
    {
        var response = await _mealService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<MealDto>>> Update(int id, [FromBody] UpdateMealDto updateDto)
    {
        updateDto.Id = id;
        var response = await _mealService.UpdateAsync(updateDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var response = await _mealService.DeleteAsync(id);
        return Ok(response);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using React.BLL.Common;
using React.BLL.DTOs.Dashboard;
using React.BLL.Interfaces;

namespace React.API.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> GetDashboard()
    {
        var response = await _dashboardService.GetDashboardAsync();
        return Ok(response);
    }
}

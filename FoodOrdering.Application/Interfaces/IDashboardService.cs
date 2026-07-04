using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Dashboard;

namespace FoodOrdering.Application.Interfaces;

public interface IDashboardService
{
    Task<ApiResponse<DashboardDto>> GetDashboardAsync();
}

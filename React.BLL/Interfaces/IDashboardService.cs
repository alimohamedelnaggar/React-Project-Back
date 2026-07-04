using React.BLL.Common;
using React.BLL.DTOs.Dashboard;

namespace React.BLL.Interfaces;

public interface IDashboardService
{
    Task<ApiResponse<DashboardDto>> GetDashboardAsync();
}

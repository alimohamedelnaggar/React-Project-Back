using AutoMapper;
using Microsoft.AspNetCore.Identity;
using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Dashboard;
using FoodOrdering.Application.Interfaces;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Domain.Interfaces;

namespace FoodOrdering.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<ApiResponse<DashboardDto>> GetDashboardAsync()
    {
        var totalCategories = await _unitOfWork.Categories.CountAsync();
        var totalMeals = await _unitOfWork.Meals.CountAsync();
        var totalOrders = await _unitOfWork.Orders.CountAsync();

        var customers = await _userManager.GetUsersInRoleAsync("Customer");
        var totalCustomers = customers.Count;

        var dashboard = new DashboardDto
        {
            TotalCategories = totalCategories,
            TotalMeals = totalMeals,
            TotalCustomers = totalCustomers,
            TotalOrders = totalOrders
        };

        return ApiResponse<DashboardDto>.SuccessResult(dashboard);
    }
}

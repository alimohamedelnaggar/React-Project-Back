using FoodOrdering.Domain.Entities;

namespace FoodOrdering.Domain.Interfaces;

public interface IMealRepository : IGenericRepository<Meal>
{
    Task<(IEnumerable<Meal> Items, int TotalCount)> GetPagedAsync(
        int pageIndex, int pageSize, string? search = null, int? categoryId = null);
    Task<Meal?> GetMealWithCategoryAsync(int id);
}

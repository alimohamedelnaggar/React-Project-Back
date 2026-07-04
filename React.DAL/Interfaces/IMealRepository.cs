using React.DAL.Entities;

namespace React.DAL.Interfaces;

public interface IMealRepository : IGenericRepository<Meal>
{
    Task<(IEnumerable<Meal> Items, int TotalCount)> GetPagedAsync(
        int pageIndex, int pageSize, string? search = null, int? categoryId = null);
    Task<Meal?> GetMealWithCategoryAsync(int id);
}

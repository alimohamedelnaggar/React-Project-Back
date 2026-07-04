using Microsoft.EntityFrameworkCore;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Domain.Interfaces;
using FoodOrdering.Infrastructure.Data;

namespace FoodOrdering.Infrastructure.Repositories;

public class MealRepository : GenericRepository<Meal>, IMealRepository
{
    public MealRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<Meal> Items, int TotalCount)> GetPagedAsync(
        int pageIndex, int pageSize, string? search = null, int? categoryId = null)
    {
        var query = _dbSet.Include(m => m.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(m => m.Name.Contains(search) || m.Description.Contains(search));

        if (categoryId.HasValue)
            query = query.Where(m => m.CategoryId == categoryId.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Meal?> GetMealWithCategoryAsync(int id)
    {
        return await _dbSet.Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);
    }
}

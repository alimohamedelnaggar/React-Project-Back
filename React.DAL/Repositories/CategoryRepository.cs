using Microsoft.EntityFrameworkCore;
using React.DAL.Entities;
using React.DAL.Interfaces;
using React.DAL.Data;

namespace React.DAL.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetCategoryWithMealsAsync(int id)
    {
        return await _dbSet.Include(c => c.Meals).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<Category> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize)
    {
        var query = _dbSet.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}

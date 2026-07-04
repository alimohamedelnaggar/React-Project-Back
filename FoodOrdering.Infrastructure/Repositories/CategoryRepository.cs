using Microsoft.EntityFrameworkCore;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Domain.Interfaces;
using FoodOrdering.Infrastructure.Data;

namespace FoodOrdering.Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetCategoryWithMealsAsync(int id)
    {
        return await _dbSet.Include(c => c.Meals).FirstOrDefaultAsync(c => c.Id == id);
    }
}

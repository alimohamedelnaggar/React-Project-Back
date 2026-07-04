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
}

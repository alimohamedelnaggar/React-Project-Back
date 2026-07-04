using React.DAL.Entities;

namespace React.DAL.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryWithMealsAsync(int id);
}

using React.DAL.Entities;

namespace React.DAL.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryWithMealsAsync(int id);
    Task<(IEnumerable<Category> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize);
}

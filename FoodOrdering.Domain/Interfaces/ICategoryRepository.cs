using FoodOrdering.Domain.Entities;

namespace FoodOrdering.Domain.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryWithMealsAsync(int id);
}

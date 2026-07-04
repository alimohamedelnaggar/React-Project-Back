using React.DAL.Common;

namespace React.DAL.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public ICollection<Meal> Meals { get; set; } = new List<Meal>();
}

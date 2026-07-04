using React.DAL.Common;

namespace React.DAL.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int MealId { get; set; }
    public Meal Meal { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

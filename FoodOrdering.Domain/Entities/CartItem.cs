using FoodOrdering.Domain.Common;

namespace FoodOrdering.Domain.Entities;

public class CartItem : BaseEntity
{
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int MealId { get; set; }
    public Meal Meal { get; set; } = null!;
    public int Quantity { get; set; }
}

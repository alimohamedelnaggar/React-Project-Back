namespace React.BLL.DTOs.Cart;

public class CartItemDto
{
    public int Id { get; set; }
    public int MealId { get; set; }
    public string MealName { get; set; } = string.Empty;
    public string MealImageUrl { get; set; } = string.Empty;
    public decimal MealPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => MealPrice * Quantity;
}

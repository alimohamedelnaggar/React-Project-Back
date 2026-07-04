namespace React.BLL.DTOs.Order;

public class OrderItemDto
{
    public int Id { get; set; }
    public int MealId { get; set; }
    public string MealName { get; set; } = string.Empty;
    public string MealImageUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice => Price * Quantity;
}

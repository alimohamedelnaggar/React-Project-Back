namespace FoodOrdering.Application.DTOs.Cart;

public class CartDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    public int ItemsCount => Items.Sum(i => i.Quantity);
}

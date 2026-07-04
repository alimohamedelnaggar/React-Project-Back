using FoodOrdering.Domain.Entities;

namespace FoodOrdering.Domain.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
    Task<Order?> GetOrderWithItemsAsync(int id);
    Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync();
}

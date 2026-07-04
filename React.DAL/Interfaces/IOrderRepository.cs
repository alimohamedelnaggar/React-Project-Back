using React.DAL.Entities;

namespace React.DAL.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
    Task<Order?> GetOrderWithItemsAsync(int id);
    Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync();
}

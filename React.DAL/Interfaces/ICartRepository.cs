using React.DAL.Entities;

namespace React.DAL.Interfaces;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<Cart?> GetCartWithItemsAsync(string userId);
    Task<CartItem?> GetCartItemAsync(int cartId, int mealId);
    void DeleteCartItem(CartItem cartItem);
    void ClearCartItems(IEnumerable<CartItem> cartItems);
}

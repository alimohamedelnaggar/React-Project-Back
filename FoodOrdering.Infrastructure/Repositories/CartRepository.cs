using Microsoft.EntityFrameworkCore;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Domain.Interfaces;
using FoodOrdering.Infrastructure.Data;

namespace FoodOrdering.Infrastructure.Repositories;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Cart?> GetCartWithItemsAsync(string userId)
    {
        return await _dbSet
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Meal)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int mealId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.MealId == mealId);
    }

    public void DeleteCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public void ClearCartItems(IEnumerable<CartItem> cartItems)
    {
        _context.CartItems.RemoveRange(cartItems);
    }
}

namespace FoodOrdering.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IMealRepository Meals { get; }
    ICartRepository Carts { get; }
    IOrderRepository Orders { get; }
    Task<int> CompleteAsync();
}

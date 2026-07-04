using React.DAL.Interfaces;
using React.DAL.Data;

namespace React.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private ICategoryRepository? _categories;
    private IMealRepository? _meals;
    private ICartRepository? _carts;
    private IOrderRepository? _orders;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICategoryRepository Categories =>
        _categories ??= new CategoryRepository(_context);

    public IMealRepository Meals =>
        _meals ??= new MealRepository(_context);

    public ICartRepository Carts =>
        _carts ??= new CartRepository(_context);

    public IOrderRepository Orders =>
        _orders ??= new OrderRepository(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

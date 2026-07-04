using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using React.DAL.Entities;
using React.DAL.Data;

namespace React.DAL.Services;

public static class SeedService
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.EnsureCreatedAsync();

        string[] roles = { "Admin", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@foodordering.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");

                var adminCart = new Cart { UserId = adminUser.Id };
                context.Carts.Add(adminCart);
                await context.SaveChangesAsync();
            }
        }

        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Appetizers", Description = "Start your meal with our delicious appetizers", ImageUrl = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=400&fit=crop" },
                new() { Name = "Main Course", Description = "Hearty and fulfilling main course dishes", ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?w=400&fit=crop" },
                new() { Name = "Desserts", Description = "Sweet treats to end your meal", ImageUrl = "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=400&fit=crop" },
                new() { Name = "Beverages", Description = "Refreshing drinks and beverages", ImageUrl = "https://images.unsplash.com/photo-1544145945-f90425340c7e?w=400&fit=crop" }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            var meals = new List<Meal>
            {
                new() { Name = "Spring Rolls", Description = "Crispy vegetable spring rolls with sweet chili sauce", Price = 5.99m, ImageUrl = "https://images.unsplash.com/photo-1604909052743-94e838986d24?w=400&fit=crop", Quantity = 50, CategoryId = categories[0].Id },
                new() { Name = "Chicken Wings", Description = "Spicy BBQ chicken wings with ranch dip", Price = 8.99m, ImageUrl = "https://images.unsplash.com/photo-1608032077018-418437b9b768?w=400&fit=crop", Quantity = 40, CategoryId = categories[0].Id },
                new() { Name = "Grilled Salmon", Description = "Fresh grilled salmon with lemon herbs", Price = 15.99m, ImageUrl = "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?w=400&fit=crop", Quantity = 30, CategoryId = categories[1].Id },
                new() { Name = "Beef Steak", Description = "Tender beef steak with mushroom sauce", Price = 18.99m, ImageUrl = "https://images.unsplash.com/photo-1600891964092-4316c288032e?w=400&fit=crop", Quantity = 25, CategoryId = categories[1].Id },
                new() { Name = "Chocolate Cake", Description = "Rich chocolate layer cake with ganache", Price = 6.99m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=400&fit=crop", Quantity = 20, CategoryId = categories[2].Id },
                new() { Name = "Ice Cream", Description = "Vanilla ice cream with chocolate sauce", Price = 4.99m, ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400&fit=crop", Quantity = 60, CategoryId = categories[2].Id },
                new() { Name = "Fresh Lemonade", Description = "Freshly squeezed lemonade with mint", Price = 3.99m, ImageUrl = "https://images.unsplash.com/photo-1621263764928-df1444c5e859?w=400&fit=crop", Quantity = 100, CategoryId = categories[3].Id },
                new() { Name = "Orange Juice", Description = "Fresh orange juice pulp-free", Price = 4.49m, ImageUrl = "https://images.unsplash.com/photo-1574316075342-50c789b1f3c9?w=400&fit=crop", Quantity = 80, CategoryId = categories[3].Id }
            };

            context.Meals.AddRange(meals);
            await context.SaveChangesAsync();
        }
    }
}

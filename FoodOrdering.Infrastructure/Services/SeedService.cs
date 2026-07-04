using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Infrastructure.Data;

namespace FoodOrdering.Infrastructure.Services;

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
                new() { Name = "Appetizers", Description = "Start your meal with our delicious appetizers", ImageUrl = "https://via.placeholder.com/200?text=Appetizers" },
                new() { Name = "Main Course", Description = "Hearty and fulfilling main course dishes", ImageUrl = "https://via.placeholder.com/200?text=Main+Course" },
                new() { Name = "Desserts", Description = "Sweet treats to end your meal", ImageUrl = "https://via.placeholder.com/200?text=Desserts" },
                new() { Name = "Beverages", Description = "Refreshing drinks and beverages", ImageUrl = "https://via.placeholder.com/200?text=Beverages" }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            var meals = new List<Meal>
            {
                new() { Name = "Spring Rolls", Description = "Crispy vegetable spring rolls", Price = 5.99m, ImageUrl = "https://via.placeholder.com/200?text=Spring+Rolls", Quantity = 50, CategoryId = categories[0].Id },
                new() { Name = "Chicken Wings", Description = "Spicy BBQ chicken wings", Price = 8.99m, ImageUrl = "https://via.placeholder.com/200?text=Chicken+Wings", Quantity = 40, CategoryId = categories[0].Id },
                new() { Name = "Grilled Salmon", Description = "Fresh grilled salmon with herbs", Price = 15.99m, ImageUrl = "https://via.placeholder.com/200?text=Grilled+Salmon", Quantity = 30, CategoryId = categories[1].Id },
                new() { Name = "Beef Steak", Description = "Tender beef steak with mushroom sauce", Price = 18.99m, ImageUrl = "https://via.placeholder.com/200?text=Beef+Steak", Quantity = 25, CategoryId = categories[1].Id },
                new() { Name = "Chocolate Cake", Description = "Rich chocolate layer cake", Price = 6.99m, ImageUrl = "https://via.placeholder.com/200?text=Chocolate+Cake", Quantity = 20, CategoryId = categories[2].Id },
                new() { Name = "Ice Cream", Description = "Vanilla ice cream with chocolate sauce", Price = 4.99m, ImageUrl = "https://via.placeholder.com/200?text=Ice+Cream", Quantity = 60, CategoryId = categories[2].Id },
                new() { Name = "Fresh Lemonade", Description = "Freshly squeezed lemonade", Price = 3.99m, ImageUrl = "https://via.placeholder.com/200?text=Lemonade", Quantity = 100, CategoryId = categories[3].Id },
                new() { Name = "Orange Juice", Description = "Fresh orange juice", Price = 4.49m, ImageUrl = "https://via.placeholder.com/200?text=Orange+Juice", Quantity = 80, CategoryId = categories[3].Id }
            };

            context.Meals.AddRange(meals);
            await context.SaveChangesAsync();
        }
    }
}

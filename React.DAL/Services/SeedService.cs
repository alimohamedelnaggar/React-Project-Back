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
            var appetizersCat = new Category { Name = "Appetizers", Description = "Start your meal with our delicious appetizers", ImageUrl = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=400&h=300&fit=crop" };
            var mainCourseCat = new Category { Name = "Main Course", Description = "Hearty and fulfilling main course dishes", ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?w=400&h=300&fit=crop" };
            var dessertsCat = new Category { Name = "Desserts", Description = "Sweet treats to end your meal", ImageUrl = "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=400&h=300&fit=crop" };
            var beveragesCat = new Category { Name = "Beverages", Description = "Refreshing drinks and beverages", ImageUrl = "https://images.unsplash.com/photo-1544145945-f90425340c7e?w=400&h=300&fit=crop" };

            context.Categories.AddRange(appetizersCat, mainCourseCat, dessertsCat, beveragesCat);
            await context.SaveChangesAsync();

            var meals = new List<Meal>
            {
                new() { Name = "Spring Rolls", Description = "Crispy vegetable spring rolls with sweet chili sauce", Price = 5.99m, ImageUrl = "https://images.unsplash.com/photo-1604909052743-94e838986d24?w=400&h=300&fit=crop", Quantity = 50, CategoryId = appetizersCat.Id },
                new() { Name = "Chicken Wings", Description = "Spicy BBQ chicken wings with ranch dip", Price = 8.99m, ImageUrl = "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=400&h=300&fit=crop", Quantity = 40, CategoryId = appetizersCat.Id },
                new() { Name = "Mozzarella Sticks", Description = "Golden fried mozzarella with marinara sauce", Price = 6.99m, ImageUrl = "https://images.unsplash.com/photo-1598515214211-89d3c73ae83b?w=400&h=300&fit=crop", Quantity = 45, CategoryId = appetizersCat.Id },
                new() { Name = "Garlic Bread", Description = "Toasted bread with garlic butter and herbs", Price = 4.99m, ImageUrl = "https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=400&h=300&fit=crop", Quantity = 60, CategoryId = appetizersCat.Id },
                new() { Name = "Bruschetta", Description = "Grilled bread with tomato basil topping", Price = 5.49m, ImageUrl = "https://images.unsplash.com/photo-1572695157366-5e585ab2b69f?w=400&h=300&fit=crop", Quantity = 35, CategoryId = appetizersCat.Id },
                new() { Name = "Nachos", Description = "Crispy tortilla chips with cheese and salsa", Price = 7.99m, ImageUrl = "https://images.unsplash.com/photo-1600891964092-4316c288032e?w=400&h=300&fit=crop", Quantity = 30, CategoryId = appetizersCat.Id },
                new() { Name = "Onion Rings", Description = "Crispy battered onion rings with dipping sauce", Price = 4.99m, ImageUrl = "https://images.unsplash.com/photo-1586190848861-99aa4a171e90?w=400&h=300&fit=crop", Quantity = 55, CategoryId = appetizersCat.Id },
                new() { Name = "Stuffed Mushrooms", Description = "Mushrooms stuffed with cheese and herbs", Price = 6.49m, ImageUrl = "https://images.unsplash.com/photo-1534422298391-e4f8c172dddb?w=400&h=300&fit=crop", Quantity = 25, CategoryId = appetizersCat.Id },

                new() { Name = "Grilled Salmon", Description = "Fresh grilled salmon with lemon herbs", Price = 15.99m, ImageUrl = "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?w=400&h=300&fit=crop", Quantity = 30, CategoryId = mainCourseCat.Id },
                new() { Name = "Beef Steak", Description = "Tender beef steak with mushroom sauce", Price = 18.99m, ImageUrl = "https://images.unsplash.com/photo-1432139555190-58524dae6a55?w=400&h=300&fit=crop", Quantity = 25, CategoryId = mainCourseCat.Id },
                new() { Name = "Margherita Pizza", Description = "Classic pizza with fresh mozzarella and basil", Price = 12.99m, ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400&h=300&fit=crop", Quantity = 20, CategoryId = mainCourseCat.Id },
                new() { Name = "Classic Burger", Description = "Beef burger with cheddar and caramelized onions", Price = 10.99m, ImageUrl = "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?w=400&h=300&fit=crop", Quantity = 35, CategoryId = mainCourseCat.Id },
                new() { Name = "Caesar Salad", Description = "Crisp romaine lettuce with parmesan and croutons", Price = 9.99m, ImageUrl = "https://images.unsplash.com/photo-1514326640560-7d063ef2aed5?w=400&h=300&fit=crop", Quantity = 40, CategoryId = mainCourseCat.Id },
                new() { Name = "Chicken Alfredo", Description = "Fettuccine pasta in creamy alfredo sauce", Price = 13.99m, ImageUrl = "https://images.unsplash.com/photo-1473093295043-cdd812d0e601?w=400&h=300&fit=crop", Quantity = 25, CategoryId = mainCourseCat.Id },
                new() { Name = "Grilled Chicken", Description = "Herb-marinated grilled chicken breast", Price = 11.99m, ImageUrl = "https://images.unsplash.com/photo-1606787366850-de6330128bfc?w=400&h=300&fit=crop", Quantity = 30, CategoryId = mainCourseCat.Id },
                new() { Name = "Beef Tacos", Description = "Soft tacos with seasoned beef and fresh salsa", Price = 9.49m, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400&h=300&fit=crop", Quantity = 35, CategoryId = mainCourseCat.Id },
                new() { Name = "Shrimp Scampi", Description = "Juicy shrimp in garlic butter sauce over pasta", Price = 14.99m, ImageUrl = "https://images.unsplash.com/photo-1553621042-f6e147245754?w=400&h=300&fit=crop", Quantity = 20, CategoryId = mainCourseCat.Id },

                new() { Name = "Chocolate Cake", Description = "Rich chocolate layer cake with ganache", Price = 6.99m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=400&h=300&fit=crop", Quantity = 20, CategoryId = dessertsCat.Id },
                new() { Name = "Ice Cream", Description = "Vanilla ice cream with chocolate sauce", Price = 4.99m, ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400&h=300&fit=crop", Quantity = 60, CategoryId = dessertsCat.Id },
                new() { Name = "New York Cheesecake", Description = "Creamy cheesecake with berry compote", Price = 7.99m, ImageUrl = "https://images.unsplash.com/photo-1550304943-4f24f54ddde9?w=400&h=300&fit=crop", Quantity = 15, CategoryId = dessertsCat.Id },
                new() { Name = "Chocolate Donuts", Description = "Glazed chocolate donuts with sprinkles", Price = 3.99m, ImageUrl = "https://images.unsplash.com/photo-1562059390-a761a084768e?w=400&h=300&fit=crop", Quantity = 40, CategoryId = dessertsCat.Id },
                new() { Name = "Fruit Crepe", Description = "Thin crepe filled with fresh fruits and cream", Price = 5.99m, ImageUrl = "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=400&h=300&fit=crop", Quantity = 25, CategoryId = dessertsCat.Id },
                new() { Name = "Apple Pie", Description = "Warm apple pie with vanilla ice cream", Price = 5.49m, ImageUrl = "https://images.unsplash.com/photo-1568571780765-9276ac8b75a2?w=400&h=300&fit=crop", Quantity = 18, CategoryId = dessertsCat.Id },
                new() { Name = "Tiramisu", Description = "Classic Italian tiramisu with mascarpone", Price = 6.99m, ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400&h=300&fit=crop", Quantity = 15, CategoryId = dessertsCat.Id },

                new() { Name = "Fresh Lemonade", Description = "Freshly squeezed lemonade with mint", Price = 3.99m, ImageUrl = "https://images.unsplash.com/photo-1621263764928-df1444c5e859?w=400&h=300&fit=crop", Quantity = 100, CategoryId = beveragesCat.Id },
                new() { Name = "Orange Juice", Description = "Fresh orange juice pulp-free", Price = 4.49m, ImageUrl = "https://images.unsplash.com/photo-1594631252845-29fc4cc8cde9?w=400&h=300&fit=crop", Quantity = 80, CategoryId = beveragesCat.Id },
                new() { Name = "Iced Coffee", Description = "Chilled coffee with milk and ice", Price = 4.99m, ImageUrl = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=400&h=300&fit=crop", Quantity = 70, CategoryId = beveragesCat.Id },
                new() { Name = "Strawberry Smoothie", Description = "Fresh strawberry and banana smoothie", Price = 5.49m, ImageUrl = "https://images.unsplash.com/photo-1572442388796-11668a67e53d?w=400&h=300&fit=crop", Quantity = 50, CategoryId = beveragesCat.Id },
                new() { Name = "Green Smoothie", Description = "Kale spinach and apple power blend", Price = 5.99m, ImageUrl = "https://images.unsplash.com/photo-1529692236671-f1f6cf9683ba?w=400&h=300&fit=crop", Quantity = 45, CategoryId = beveragesCat.Id },
                new() { Name = "Iced Tea", Description = "Refreshing peach iced tea", Price = 3.49m, ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400&h=300&fit=crop", Quantity = 90, CategoryId = beveragesCat.Id }
            };

            context.Meals.AddRange(meals);
            await context.SaveChangesAsync();
        }
    }
}

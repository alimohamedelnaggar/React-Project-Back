using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodOrdering.Domain.Entities;

namespace FoodOrdering.Infrastructure.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");
        builder.HasKey(ci => ci.Id);

        builder.HasOne(ci => ci.Cart)
               .WithMany(c => c.CartItems)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.Meal)
               .WithMany(m => m.CartItems)
               .HasForeignKey(ci => ci.MealId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

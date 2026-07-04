using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodOrdering.Domain.Entities;

namespace FoodOrdering.Infrastructure.Data.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.ToTable("Meals");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Description).HasMaxLength(500);
        builder.Property(m => m.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(m => m.ImageUrl).HasMaxLength(500);
        builder.Property(m => m.Quantity).IsRequired();

        builder.HasOne(m => m.Category)
               .WithMany(c => c.Meals)
               .HasForeignKey(m => m.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

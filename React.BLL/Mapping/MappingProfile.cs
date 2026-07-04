using AutoMapper;
using React.BLL.DTOs.Cart;
using React.BLL.DTOs.Category;
using React.BLL.DTOs.Meal;
using React.BLL.DTOs.Order;
using React.DAL.Entities;

namespace React.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.MealsCount, opt => opt.Ignore());
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<Meal, MealDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<CreateMealDto, Meal>();
        CreateMap<UpdateMealDto, Meal>();

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.MealName, opt => opt.MapFrom(src => src.Meal.Name))
            .ForMember(dest => dest.MealImageUrl, opt => opt.MapFrom(src => src.Meal.ImageUrl))
            .ForMember(dest => dest.MealPrice, opt => opt.MapFrom(src => src.Meal.Price));

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.MealName, opt => opt.MapFrom(src => src.Meal.Name))
            .ForMember(dest => dest.MealImageUrl, opt => opt.MapFrom(src => src.Meal.ImageUrl));
    }
}

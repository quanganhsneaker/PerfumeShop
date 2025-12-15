using AutoMapper;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Models;
namespace PerfumeShop.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<Order, CheckoutDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemVM>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.Product.ImageUrl));
            CreateMap<Order, OrderDetailVM>()
            .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems));
            CreateMap<Order, OrderListVM>();
            CreateMap<Order, AdminOrderListVM>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.User.FullName));        
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<Order, AdminOrderDetailVM>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.User.FullName))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems));
            CreateMap<Product, ProductListVM>()
            .ForMember(d => d.CategoryName,
               o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Rating,
               o => o.MapFrom(s => s.Rating));
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Product, ProductUpdateDto>();
            CreateMap<Order, AdminOrderListVM>()
            .ForMember(dest => dest.CustomerName,
            opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<CheckoutDto, Order>()
            .ForMember(d => d.Status, o => o.MapFrom(_ => "Pending"))
            .ForMember(d => d.TotalAmount, o => o.Ignore())   
            .ForMember(d => d.UserId, o => o.Ignore())        
            .ForMember(d => d.OrderCode, o => o.Ignore());
            CreateMap<CartItem, OrderItem>()
           .ForMember(d => d.Id, o => o.Ignore())
           .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
           .ForMember(d => d.Price, o => o.MapFrom(s => s.Product.Price))
           .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Qty))
           .ForMember(d => d.OrderId, o => o.Ignore());
            CreateMap<ReviewDto, Review>()
            .ForMember(r => r.Id, o => o.Ignore())
            .ForMember(r => r.UserId, o => o.Ignore())
            .ForMember(r => r.CreatedAt, o => o.Ignore());
            CreateMap<CategoryDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<Category, CategoryUpdateDto>();




        }
    }
}

using AutoMapper;
using PerfumeShop.DTOs;
using PerfumeShop.Models;

namespace PerfumeShop.Mappings
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

        }



    }

}

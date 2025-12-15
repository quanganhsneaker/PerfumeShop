using AutoMapper;
using FluentAssertions;
using Moq;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Application.Orders.User.Commands;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using Xunit;

namespace PerfumeShop.Tests.Orders
{
    public class CreateOrderHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepo;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IOrderCodeService> _orderCode;
        private readonly IMapper _mapper;

        public CreateOrderHandlerTests()
        {
            _orderRepo = new Mock<IOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            _orderCode = new Mock<IOrderCodeService>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CheckoutDto, Order>()
                    .ForMember(d => d.Status, o => o.MapFrom(_ => "Pending"))
                    .ForMember(d => d.TotalAmount, o => o.Ignore());

                cfg.CreateMap<CartItem, OrderItem>()
                    .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
                    .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Qty))
                    .ForMember(d => d.Price, o => o.MapFrom(s => s.Product.Price));
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Should_Create_Order_Successfully()
        {
            // Arrange
            var cart = new Cart
            {
                UserId = 1,
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductId = 5,
                        Qty = 2,
                        Product = new Product
                        {
                            Id = 5,
                            Price = 100000
                        }
                    }
                }
            };

            _orderRepo
                .Setup(x => x.GetCartByUserIdAsync(1))
                .ReturnsAsync(cart);

            _orderCode
                .Setup(x => x.GenerateOrderCode(It.IsAny<int>()))
                .Returns("DH0001");

            _uow
                .Setup(x => x.SaveChangesAsync(default))
                .ReturnsAsync(1);

            var handler = new CreateOrderHandler(
                _orderRepo.Object,
                _uow.Object,
                _orderCode.Object,
                _mapper
            );

            var command = new CreateOrderCommand(
                new CheckoutDto
                {
                    FullName = "A",
                    Phone = "123",
                    Address = "HN"
                },
                1
            );

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeGreaterThan(0);

            _orderRepo.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
            _orderRepo.Verify(x => x.AddItemsAsync(It.IsAny<List<OrderItem>>()), Times.Once);
            _orderRepo.Verify(x => x.RemoveCartAsync(cart), Times.Once);
        }

        [Fact]
        public async Task Should_Fail_When_Cart_Is_Empty()
        {
            _orderRepo
                .Setup(x => x.GetCartByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Cart?)null);

            var handler = new CreateOrderHandler(
                _orderRepo.Object,
                _uow.Object,
                _orderCode.Object,
                _mapper
            );

            var command = new CreateOrderCommand(new CheckoutDto(), 999);

            var result = await handler.Handle(command, default);

            result.Should().Be(-1);
        }
    }
}

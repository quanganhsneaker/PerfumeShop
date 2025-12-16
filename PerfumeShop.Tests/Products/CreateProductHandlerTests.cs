using AutoMapper;
using FluentAssertions;
using Moq;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Application.Products.Commands;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using Xunit;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo;
    private readonly Mock<IFileStorageService> _fileStorage;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly IMapper _mapper;

    public CreateProductHandlerTests()
    {
        _productRepo = new Mock<IProductRepository>();
        _fileStorage = new Mock<IFileStorageService>();
        _uow = new Mock<IUnitOfWork>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductCreateDto, Product>();
        });

        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task Should_Create_Product()
    {
        // Arrange
        _productRepo
            .Setup(x => x.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(p => p.Id = 1)
            .Returns(Task.CompletedTask);

        _uow
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateProductHandler(
            _productRepo.Object,
            _fileStorage.Object,
            _mapper,
            _uow.Object
        );

        var command = new CreateProductCommand(
            new ProductCreateDto
            {
                Name = "Test Perfume",
                Description = "Nice smell",
                Price = 150000
            },
            null
        );

        // Act
        var id = await handler.Handle(command, default);

        // Assert
        id.Should().Be(1);

        _productRepo.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
        _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}

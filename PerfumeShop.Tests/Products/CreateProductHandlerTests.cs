using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Application.Products.Commands;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;
using Xunit;

public class CreateProductHandlerTests
{
    private readonly ApplicationDbContext _db;
    private readonly Mock<IWebHostEnvironment> _env;
    private readonly IMapper _mapper;

    public CreateProductHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("CreateProductDb")
            .Options;

        _db = new ApplicationDbContext(options);

        _env = new Mock<IWebHostEnvironment>();
        _env.Setup(e => e.WebRootPath).Returns("wwwroot");

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductCreateDto, Product>();
        });

        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task Should_Create_Product()
    {
        var handler = new CreateProductHandler(_db, _env.Object, _mapper);

        var command = new CreateProductCommand(
            new ProductCreateDto
            {
                Name = "Test Perfume",
                Description = "Nice smell",
                Price = 150000
            },
            null
        );

        var id = await handler.Handle(command, default);

        id.Should().BeGreaterThan(0);

        var product = await _db.Products.FindAsync(id);
        product.Name.Should().Be("Test Perfume");
        product.Price.Should().Be(150000);
    }
}

//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using PerfumeShop.Application.DTOs;
//using PerfumeShop.Application.Mappings;
//using PerfumeShop.Application.Products.Commands;
//using PerfumeShop.Domain.Models;
//using PerfumeShop.Infrastructure.Data;
//using Xunit;


//public class UpdateProductHandlerTests
//{
//    private readonly ApplicationDbContext _db;
//    private readonly Mock<IWebHostEnvironment> _env;
//    private readonly IMapper _mapper;

//    public UpdateProductHandlerTests()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase("UpdateProductDb")
//            .Options;

//        _db = new ApplicationDbContext(options);

//        _env = new Mock<IWebHostEnvironment>();
//        _env.Setup(e => e.WebRootPath).Returns("wwwroot");

//        var config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile<AutoMapperProfile>();
//        });

//        _mapper = config.CreateMapper();

//    }


//    [Fact]
//    public async Task Should_Update_Product_Without_Image()
//    {
//        var existing = new Product
//        {
//            Id = 1,
//            Name = "Old Name",
//            Description = "Old Desc",
//            Price = 100000,
//            ImageUrl = "/images/products/old.png"
//        };

//        _db.Products.Add(existing);
//        await _db.SaveChangesAsync();

//        var handler = new UpdateProductHandler(_db, _env.Object, _mapper);

//        var cmd = new UpdateProductCommand(
//            new ProductUpdateDto
//            {
//                Id = 1,
//                Name = "New Name",
//                Description = "New Desc",
//                Price = 200000
//            },
//            null // không upload ảnh
//        );

//        var result = await handler.Handle(cmd, default);

//        result.Should().BeTrue();

//        var updated = await _db.Products.FindAsync(1);
//        updated.Name.Should().Be("New Name");
//        updated.Price.Should().Be(200000);
//        updated.ImageUrl.Should().Be("/images/products/old.png");
//    }


   
//    [Fact]
//    public async Task Should_Fail_When_Product_Not_Found()
//    {
//        var handler = new UpdateProductHandler(_db, _env.Object, _mapper);

//        var cmd = new UpdateProductCommand(
//            new ProductUpdateDto
//            {
//                Id = 999,
//                Name = "X",
//                Price = 1000
//            },
//            null
//        );

//        var result = await handler.Handle(cmd, default);

//        result.Should().BeFalse();
//    }
//}

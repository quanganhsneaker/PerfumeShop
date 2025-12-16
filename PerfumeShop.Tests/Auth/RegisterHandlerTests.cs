using AutoMapper;
using FluentAssertions;
using Moq;
using PerfumeShop.Application.Auth.Commands.Register;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using Xunit;

public class RegisterHandlerTests
{
    private readonly Mock<IUserRepository> _userRepo;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly IMapper _mapper;

    public RegisterHandlerTests()
    {
        _userRepo = new Mock<IUserRepository>();
        _uow = new Mock<IUnitOfWork>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RegisterDto, User>();
        });

        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task Should_Register_Successfully()
    {
        // Arrange
        _userRepo
            .Setup(x => x.ExistsByEmailAsync("test@gmail.com"))
            .ReturnsAsync(false);

        _userRepo
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        _uow
            .Setup(x => x.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var handler = new RegisterHandler(
            _userRepo.Object,
            _uow.Object,
            _mapper
        );

        var command = new RegisterCommand(new RegisterDto
        {
            FullName = "Test User",
            Email = "test@gmail.com",
            Password = "123456"
        });

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().BeTrue();

        _userRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        _uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_When_Email_Already_Exists()
    {
        // Arrange
        _userRepo
            .Setup(x => x.ExistsByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var handler = new RegisterHandler(
            _userRepo.Object,
            _uow.Object,
            _mapper
        );

        var command = new RegisterCommand(new RegisterDto
        {
            Email = "exist@gmail.com",
            Password = "123456"
        });

     
        var result = await handler.Handle(command, default);

       
        result.Should().BeFalse();

        _userRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        _uow.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}

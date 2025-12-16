using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using PerfumeShop.Application.Auth.Commands.Login;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using Xunit;

public class LoginHandlerTests
{
    private readonly Mock<IUserRepository> _userRepo;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly IMemoryCache _cache;

    public LoginHandlerTests()
    {
        _userRepo = new Mock<IUserRepository>();
        _uow = new Mock<IUnitOfWork>();
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    [Fact]
    public async Task Should_Login_Successfully()
    {
        // Arrange
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");

        _userRepo
            .Setup(x => x.GetByEmailAsync("test@gmail.com"))
            .ReturnsAsync(new User
            {
                Id = 1,
                Email = "test@gmail.com",
                PasswordHash = passwordHash
            });

        var handler = new LoginHandler(
            _userRepo.Object,
            _uow.Object,
            _cache
        );

        var command = new LoginCommand(new LoginDto
        {
            Email = "test@gmail.com",
            Password = "123456",
            RememberMe = true
        });

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Success.Should().BeTrue();
        result.User.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Fail_When_Password_Is_Wrong()
    {
        // Arrange
        _userRepo
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new User
            {
                Email = "test@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct")
            });

        var handler = new LoginHandler(
            _userRepo.Object,
            _uow.Object,
            _cache
        );

        var command = new LoginCommand(new LoginDto
        {
            Email = "test@gmail.com",
            Password = "wrong"
        });

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Sai email hoặc mật khẩu.");
    }

    [Fact]
    public async Task Should_Lock_Account_After_5_Failed_Attempts()
    {
        // Arrange
        var email = "lock@gmail.com";
        var cacheKey = $"login_failed_{email}";

        _cache.Set(cacheKey, 5, TimeSpan.FromMinutes(5));

        var handler = new LoginHandler(
            _userRepo.Object,
            _uow.Object,
            _cache
        );

        var command = new LoginCommand(new LoginDto
        {
            Email = email,
            Password = "123"
        });
        var result = await handler.Handle(command, default);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Contain("bị khóa");
    }
}

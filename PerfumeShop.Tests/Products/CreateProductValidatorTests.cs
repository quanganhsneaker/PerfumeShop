using Xunit;
using FluentValidation.TestHelper;
using PerfumeShop.Application.Products.Commands;
using PerfumeShop.Application.DTOs;

public class CreateProductValidatorTests
{
    private readonly CreateProductValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateProductCommand(
            new ProductCreateDto { Name = "" },
            null
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var command = new CreateProductCommand(
            new ProductCreateDto
            {
                Name = "Perfume A",
                Description = "Luxury",
                Price = 200000
            },
            null
        );

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}

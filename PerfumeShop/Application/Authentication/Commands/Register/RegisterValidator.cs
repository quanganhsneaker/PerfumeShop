using FluentValidation;

namespace PerfumeShop.Application.Auth.Commands.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Dto.FullName)
                .NotEmpty().WithMessage("Họ tên không được để trống");

            RuleFor(x => x.Dto.Email)
                .NotEmpty().EmailAddress();

            RuleFor(x => x.Dto.Password)
                .MinimumLength(6)
                .WithMessage("Mật khẩu tối thiểu 6 ký tự");
        }
    }
}


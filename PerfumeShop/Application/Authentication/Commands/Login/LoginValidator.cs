using FluentValidation;

namespace PerfumeShop.Application.Auth.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");

            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống");
        }
    }
}

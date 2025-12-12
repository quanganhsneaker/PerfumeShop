using FluentValidation;

namespace PerfumeShop.Application.Orders.User.Commands
{
    public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(x => x.Dto.FullName)
                .NotEmpty().WithMessage("Họ tên không được để trống.");

            RuleFor(x => x.Dto.Phone)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^\d{9,11}$").WithMessage("Số điện thoại không hợp lệ.");

            RuleFor(x => x.Dto.Address)
                .NotEmpty().WithMessage("Bạn chưa nhập địa chỉ giao hàng.");

            RuleFor(x => x.Dto.PaymentMethod)
                .NotEmpty().WithMessage("Bạn chưa chọn phương thức thanh toán.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId không hợp lệ.");
        }
    }
}

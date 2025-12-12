using FluentValidation;

namespace PerfumeShop.Application.Orders.User.Commands
{
    public class CreateOrderValidator : AbstractValidator<CheckoutCommand>

    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Dto.FullName)
                .NotEmpty().WithMessage("Họ và tên không được để trống.");
            RuleFor(x => x.Dto.Phone)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^\d{9,11}$").WithMessage("Số điện thoại không hợp lệ.");
            RuleFor(x => x.Dto.Address)
                .NotEmpty().WithMessage("Địa chỉ không được để trống vui lòng nhập");
            RuleFor(x => x.Dto.PaymentMethod)
                .NotEmpty().WithMessage("Bạn chưa chọn phương thức thanh toán");
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Người dùng không hợp lệ");


           
                
                }
    }
}

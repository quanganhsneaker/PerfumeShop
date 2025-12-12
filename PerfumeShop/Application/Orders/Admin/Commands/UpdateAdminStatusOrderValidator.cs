using FluentValidation;

namespace PerfumeShop.Application.Orders.Admin.Commands
{
    public class UpdateAdminStatusOrderValidator
        : AbstractValidator<UpdateAdminStatusOrderCommand>
    {
        public UpdateAdminStatusOrderValidator()
        {
           
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("OrderId không hợp lệ.");

          
            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Trạng thái đơn hàng không được bỏ trống.");

        
            RuleFor(x => x.Status)
                .Must(IsValidStatus)
                .WithMessage("Trạng thái không hợp lệ. Các trạng thái hợp lệ: Pending, Processing, Shipped, Completed, Cancelled.");
        }

        private bool IsValidStatus(string status)
        {
            string[] validStatuses =
            {
                "Pending",
                "Processing",
                "Shipped",
                "Completed",
                "Cancelled"
            };

            return validStatuses.Contains(status);
        }
    }
}

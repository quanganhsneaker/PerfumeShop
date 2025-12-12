using FluentValidation;

namespace PerfumeShop.Application.Reviews.Commands
{
    public class AddReviewValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewValidator()
        {
            RuleFor(x => x.Dto.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating phải từ 1 đến 5.");

            RuleFor(x => x.Dto.Comment)
                .NotEmpty()
                .WithMessage("Vui lòng nhập nội dung đánh giá.");
        }
    }
}

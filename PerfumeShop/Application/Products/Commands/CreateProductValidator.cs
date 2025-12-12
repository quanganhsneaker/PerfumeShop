using FluentValidation;

namespace PerfumeShop.Application.Products.Commands
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {

            RuleFor(x => x.Dto)
                .NotNull()
                .WithMessage("Dữ liệu sản phẩm không hợp lệ.");
            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Tên sản phẩm không được để trống.")
                .MaximumLength(200).WithMessage("Tên sản phẩm không được vượt quá 200 ký tự.");
            RuleFor(x => x.Dto.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.");
            RuleFor(x => x.Dto.Price)
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0.");
            RuleFor(x => x.ImageFile)
                .Must(f => f == null || f.Length > 0)
                .WithMessage("Ảnh tải lên không hợp lệ.");
            RuleFor(x => x.ImageFile)
                .Must(f => f == null || IsValidImage(f.FileName))
                .WithMessage("Ảnh phải thuộc định dạng .jpg, .png, .jpeg.");
        }
        private bool IsValidImage(string fileName)
        {
            string[] ext = { ".jpg", ".jpeg", ".png" };
            return ext.Contains(Path.GetExtension(fileName).ToLower());
        }
    }
}

using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class PostValidator: AbstractValidator<PostEditModel>
    {
        public PostValidator()
        {
            RuleFor(a => a.Title)
                .NotEmpty()
                .WithMessage("Tên bài viết không được để trống")
                .MaximumLength(100)
                .WithMessage("Tên bài viết tối đa 100 ký tự");

            RuleFor(a => a.ShortDescription)
                .NotEmpty()
                .WithMessage("Mô tả ngắn không được để trống")
                .MaximumLength(200)
                .WithMessage("Môt tả ngắn tối đã 200 ký tự");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Mô tả không được để trống")
                .MaximumLength(2000)
                .WithMessage("Mô tả tối đa 2000 ký tự");

            RuleFor(a => a.Meta)
                .NotEmpty()
                .WithMessage("Meta không được để trống")
                .MaximumLength(50)
                .WithMessage("Meta không quá 50 ký tự");

            RuleFor(a => a.UrlSlug)
                .NotEmpty()
                .WithMessage("URL không được để trống")
                .MaximumLength(100)
                .WithMessage("URL không quá 100 ký tự");
        }
    }
}

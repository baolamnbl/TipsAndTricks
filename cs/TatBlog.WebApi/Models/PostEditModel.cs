using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TatBlog.Core.Entities;

namespace TatBlog.WebApi.Models
{
    public class PostEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tiêu đề")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(500, ErrorMessage = "Tiêu đề tối đa 500 kí tự")]
        public string Title { get; set; }

        [DisplayName("Giới thiệu")]
        [Required]

        //test
        //public string keywork { get; set; }
        //[DisplayName("Từ khóa")]
        //[Required]

        public string UrlSlug { get; set; }
        [Required]
        [DisplayName("UrlSlug")]
        public string ShortDescription { get; set; }

        [DisplayName("Nội dung")]
        [Required]
        public string Description { get; set; }

        [DisplayName("Metadata")]
        [Required]
        public string Meta { get; set; }

        [DisplayName("Chọn hình ảnh")]
        [Required]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Hình hiện tại")]
        [Required]
        public string ImageUrl { get; set; }

        [DisplayName("Xuất bản ngày")]
        [Required]
        public bool Published { get; set; }

        [DisplayName("Chủ đề")]
        [Required]
        public int CategoryId { get; set; }

        [DisplayName("Tác giả")]
        [Required]
        public int AuthorId { get; set; }

        [DisplayName("Từ khóa (Mỗi từ 1 dòng)")]
        [Required]
        public string SelectedTags { get; set; }

        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public List<string> GetSelectedTags()
        {
            return (SelectedTags ?? "")
                .Split(new[] { ',', ';', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        public static async ValueTask<PostEditModel> BindAsync(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();
            return new PostEditModel()
            {
                ImageFile = form.Files["ImageFile"],
                Id = int.Parse(form["Id"]),
                Title = form["Title"],
                ShortDescription = form["ShortDescription"],
                Description = form["Description"],
                Meta = form["Meta"],
                Published = form["Published"] != "false",
                CategoryId = int.Parse(form["CategoryId"]),
                AuthorId = int.Parse(form["AuthorId"]),
                SelectedTags = form["SelectedTags"]
            };
        }




        //public string ImageUrl { get; set; }
        //public int ViewCount { get; set; }
        //public bool Published { get; set; }
        //public DateTime PostedDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public int CategoryId { get; set; }
        //public int AuthorId { get; set; }
        //public Category Category { get; set; }
        //public Author Author { get; set; }
    }
}

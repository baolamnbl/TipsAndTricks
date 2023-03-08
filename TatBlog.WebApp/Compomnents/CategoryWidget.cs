using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Compomnents
{
    public class CategoryWidget:ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public CategoryWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _blogRepository.GetCategoriesAsync();
            return View(categories);
        }
    }
}

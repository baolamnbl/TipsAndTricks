using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        //public IActionResult Index()
        //{
            private readonly IBlogRepository _blogRepository;
            public PostsController(IBlogRepository blogRepository)
            {
                _blogRepository = blogRepository;
            }

            public async Task<IActionResult> Index(PostFilterModel model)
            {
                var postQuery = new PostQuery()
                {
                keyWord = model.Keyword,
                categoryId = model.CategoryId,
                authorId = model.AuthorId,
                postYear = model.Year,
                postMonth = model.Month
                };
                ViewBag.PostsList = await _blogRepository
                    .GetPagedPostsAsync(postQuery, 1,10);
                await PopulatePostFilterModelAsync(model);
                return View(model);
            }
            private async Task PopulatePostFilterModelAsync(PostFilterModel model)
            {
                var authors = await _blogRepository.GetAuthorsAsync();
                var categories = await _blogRepository.GetCategoriesAsync();

                model.AuthorList = authors.Select(a => new SelectListItem()
                {
                    Text = a.FullName,
                    Value = a.Id.ToString(),
                });

                model.CategoryList = categories.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });
            }
        
    }
}
 
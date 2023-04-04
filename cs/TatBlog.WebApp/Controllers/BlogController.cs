﻿using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Properties.Controllers
{
    public class BlogController:Controller
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
      
            
        
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

        public async Task<IActionResult> Index(
            //[FromQuery(Name ="k")]string keywork=null,
            [FromQuery(Name ="p")]int pageNumber=1,
            [FromQuery(Name ="ps")]int pageSize=10)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
               // keyWord = keywork
            };
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery= postQuery;
            return View(postsList);
        }
        public async Task<IActionResult> Post(
            string slug, int month, int year)
        {
            var post = await _blogRepository.GetPostAsync(year, month, slug);
            return View(post);
        }
    }

    
}

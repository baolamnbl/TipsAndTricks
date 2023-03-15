﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class PostEditModel
    {
        public int Id { get; set; }
        [DisplayName("Tiêu đề")]

        public string  ? Title { get; set; }
        [DisplayName("Giới thiệu")]
        
        public string? ShortDescription { get; set; }
        [DisplayName("Nội dung")]
        
        public string? Description { get; set; }
        [DisplayName("Metadata")]
        
        public string? Meta { get; set; }
        [DisplayName("Slug")]
        [Remote("VerifyPostSlug","Posts","Admin",HttpMethod ="POST",AdditionalFields ="Id")]
        
        public string? UrlSlug { get; set; }
        [DisplayName("Chọn hình ảnh")]
        public IFormFile? ImageFile { get; set; }
        [DisplayName("Hình hiện tại")]
        public string? ImageUrl { get; set; }
        [DisplayName("Xuất bản ngay")]
        public bool Published { get; set; }
        [DisplayName("Chủ đề")]
        
        public int CategoryId { get; set; }
        [DisplayName("Tác giả")]
        
        public bool AuthorId { get; set; }
        [DisplayName("Từ khoá(mỗi từ 1 dòng)")]
        
        public string? SelectedTags { get; set; }

        public IEnumerable<SelectListItem>? AuthorList { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public List<string> GetSelectedTags()
        {
            return (SelectedTags ?? "")
                .Split(new[] {',', ';' , '\r', '\n'},
                StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }


    }
}

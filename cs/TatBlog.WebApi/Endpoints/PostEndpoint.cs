using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoint
    {
        public static WebApplication MapPostEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPosts)
                .WithName("GetPosts")
                .Produces<ApiResponse<PaginationResult<PostItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                .WithName("GetPostById")
                .Produces<ApiResponse<PostItem>>();

            //routeGroupBuilder.MapGet(
            //	"/byslug/{slug:regex(^[a-z0-9 -]+$)}",
            //	GetPostsByAuthorSlug)
            //	.WithName("GetPostsByAuthorSlug")
            //	.Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddNewPost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<PostItem>>();

            routeGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
                .WithName("SetPostPicture")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                .WithName("UpdatePost")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetPosts(
            [AsParameters] PostFilterModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var postQuery = mapper.Map<PostQuery>(model);
            var postList = await blogRepository.GetPagedPostsAsync(postQuery, model,
                p => p.ProjectToType < PostDto>());

            var Page = new PaginationResult<PostDto>(postList);
            return Results.Ok(ApiResponse.Success(Page));
        }

        private static async Task<IResult> GetPostDetails(
            int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var post = await blogRepository.GetCachedPostByIdAsync(id);

            return post == null
                ? Results.Ok(ApiResponse.Fail(System.Net.HttpStatusCode.NotFound,
                $"Không tìm thấy bài viết có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(post)));
        }

        //
        private static async Task<IResult> GetPostsByAuthorId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                authorId = id,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        public static async Task<IResult> GetPostsByAuthorSlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        //

        private static async Task<IResult> AddPost(
            PostEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository
                .IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            await blogRepository.AddOrUpdateAsync(post);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<PostItem>(post), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetPostPicture(
            int id, IFormFile imageFile,
            IBlogRepository blogRepository,
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName,
                imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await blogRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> UpdatePost(
            int id, PostEditModel model,
            IBlogRepository blogRepository,
            IValidator<PostEditModel> validator,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest,
                    validationResult));
            }

            if (await blogRepository
                .IsPostSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            post.Id = id;

            return await blogRepository.AddOrUpdateAsync(post)
                ? Results.Ok(ApiResponse.Success("Post is updated",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.NotFound, "Could not find post"));
        }

        private static async Task<IResult> DeletePost(
            int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePostAsync(id)
                ? Results.Ok(ApiResponse.Success("Post is deleted",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find post"));
        }
    }
}

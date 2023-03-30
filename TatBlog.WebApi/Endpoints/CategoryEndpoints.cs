using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategory)
                .WithName("GetCategory")
                .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<AuthorItem>>()
                .Produces(404);

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlug")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            //routeGroupBuilder.MapPost("/", AddCategory)
            //    .WithName("AddCategory")
            //    .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
            //    .Produces(401)
            //    .Produces<ApiResponse<CategoryItem>>();

            //routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
            //    .WithName("UpdateCategory")
            //    .Produces(401)
            //    .Produces<ApiResponse<string>>();
            return app;
        }
        private static async Task<IResult> GetCategory([AsParameters] CategoryFilterModel model,
           IBlogRepository blogRepository)
        {
            var categoriesList = await blogRepository.GetPagedCategorysAsync(model, model.Name);
            var paginationResult = new PaginationResult<CategoryItem>(categoriesList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> GetCategoryDetails(int id,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);
            return author == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }
        private static async Task<IResult> GetPostsByCategorySlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(paginationResult);
        }
        private static async Task<IResult> AddCategory(
            CategoryEditModel model,
            IValidator<CategoryEditModel> validator,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' da su dung"));
            }

            var category = mapper.Map<Category>(model);
            await blogRepository.AddOrUpdateAsync(category);
            return Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
        }
        private static async Task<IResult> UpdateCategory(int id, CategoryEditModel model, IValidator<CategoryEditModel> validator, IBlogRepository blogRepository, IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
            }
            if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug 'model.UrlSlug' da duoc su dung"));
            }
            var category = mapper.Map<Category>(model);
            category.Id = id;
            return await blogRepository.AddOrUpdateAsync(category)
                ? Results.Ok(ApiResponse.Success("Author is update", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Couldn't find category"));

        }
    }
}

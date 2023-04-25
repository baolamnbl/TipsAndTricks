using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken= default);

        Task <bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken=default);

        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        Task<Tag> GetTagFromSlugAsync(
            string slug,
            CancellationToken cancellationToken = default);
        Task<IPagedList> GetPagedPostsAsync(
            PostQuery condition, int pageNumber, int pageSize,
            CancellationToken cancellationToken = default);
        Task<IPagedList<T>> GetPagedPostsAsync<T>(
            PostQuery condition, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper,
            CancellationToken cancellationToken = default);

        Task GetAuthorAsync();
        //Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);
        Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default);
        Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default);
        Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default);
        Task<IList<Post>> GetFeaturePostAysnc(
          int numberPost,
          CancellationToken cancellationToken = default);
        Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default);
        Task<IPagedList<T>> GetPagedCategorysAsync<T>(
        Func<IQueryable<Category>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);
        Task<IPagedList<CategoryItem>> GetPagedCategorysAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);
        Task<bool> AddOrUpdateAsync(
        Category category, CancellationToken cancellationToken = default);
        Task<Post> GetCachedPostByIdAsync(int postId);
        //Task AddOrUpdateAsync(Post post);
        Task<bool> SetImageUrlAsync(
        int postId, string imageUrl,
        CancellationToken cancellationToken = default);
        Task<bool> DeletePostAsync(int postId, CancellationToken cancellationToken = default);
        Task<bool> AddOrUpdateAsync(Post post, CancellationToken cancellationToken = default);
        Task<bool> IsCategoryExistSlugAsync(int id, string slug, CancellationToken cancellationToken = default);
    }
}

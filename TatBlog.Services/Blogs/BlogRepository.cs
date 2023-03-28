using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;
        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x=>x.Tags);
            if (year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == slug);
            }
            return await postQuery.FirstOrDefaultAsync(cancellationToken);

        }

        public async Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }
        public async Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }



        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();
            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }
            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }
        //public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        //{
        //    IQueryable<Author> authors = _context.Set<Author>();
            
        //    return await authors
        //        .OrderBy(x => x.FullName)
        //        .Select(x => new AuthorItem()
        //        {
        //            Id = x.Id,
        //            FullName = x.FullName,
        //            UrlSlug = x.UrlSlug,
        //            Email = x.Email,
        //            JoinedDate = x.JoinedDate,
        //            Notes = x.Notes,
        //            PostCount = x.Posts.Count(p => p.Published)

        //        })
        //        .ToListAsync(cancellationToken);
        //}
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<Tag> GetTagFromSlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(t => t.UrlSlug == slug)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IPagedList> GetPagedPostsAsync(
            PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }
        private IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.categoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.categoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.authorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.authorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.keyWord))
            {
                posts = posts.Where(x => x.Title.Contains(condition.keyWord) ||
                                         x.ShortDescription.Contains(condition.keyWord) ||
                                         x.Description.Contains(condition.keyWord) ||
                                         x.Category.Name.Contains(condition.keyWord) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.keyWord)));
            }

            if (condition.postYear > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.postYear);
            }

            if (condition.postMonth > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.postMonth);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;
        }

        public Task GetAuthorAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }


        //public Task<IPagedList> GetPagedPostsAsync(PostQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }
        public async Task<Tag> GetTagAsync(
		string slug, CancellationToken cancellationToken = default)
	    {
		return await _context.Set<Tag>()
			.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	    }
        public async Task<IList<Post>> GetFeaturePostAysnc(
          int numberPost,
          CancellationToken cancellationToken = default)
        {

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .OrderByDescending(x => x.ViewCount)
                .Take(numberPost)
                .ToListAsync(cancellationToken);
        }
        public async Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(x => Guid.NewGuid())
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }
    }
}
